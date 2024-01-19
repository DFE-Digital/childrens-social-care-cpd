# Populate a local to use if waf with grafana is to be configured in Prod or Load-Test
locals {
  waf_grafana_enabled = can(regex("^Prod|Load", terraform.workspace)) ? [] : [1]
  waf_grafana_count   = can(regex("^Prod|Load", terraform.workspace)) ? 1 : 0
}

# Definition of a service plan for a Linux Container
resource "azurerm_service_plan" "service-plan-gf" {
  name                = var.grafana_service_plan_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "B2"
  tags                = data.azurerm_resource_group.rg.tags
}

resource "azurerm_storage_account" "gfsa" {
  name                     = var.grafana_sa_name[terraform.workspace]
  resource_group_name      = data.azurerm_resource_group.rg.name
  location                 = data.azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = data.azurerm_resource_group.rg.tags
}

resource "azurerm_storage_share" "gffileshare" {
  name                 = "grafanashare"
  storage_account_name = azurerm_storage_account.gfsa.name
  quota                = 50
}

# Definition of the Grafana linux web app for the service
resource "azurerm_linux_web_app" "grafana-web-app" {
  name                = var.grafana_webapp_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  service_plan_id     = azurerm_service_plan.service-plan-gf.id

  app_settings = {
    DOCKER_ENABLE_CI                    = "true"
    GF_SECURITY_X_XSS_PROTECTION        = "false"
    GF_SERVER_DOMAIN                    = var.hostname[terraform.workspace]
    GF_LIVE_ALLOWED_ORIGINS             = "*"
    GF_SERVER_ROOT_URL                  = "%(protocol)s://%(domain)s:%(http_port)s/grafana"
    GF_SERVER_SERVE_FROM_SUB_PATH       = "true"
    GF_SECURITY_ADMIN_USER              = "admin"
    GF_SECURITY_CSRF_ADDITIONAL_HEADERS = "X-Forwarded-Host"
    GF_SECURITY_CSRF_TRUSTED_ORIGINS    = var.origins[terraform.workspace]
    GF_SECURITY_ADMIN_PASSWORD          = var.cpd_gf_password
    GF_INSTALL_PLUGINS                  = var.cpd_gf_plugins
  }

  site_config {
    application_stack {
      docker_registry_url = "https://ghcr.io"
      docker_image_name   = "dfe-digital/grafana-azurecostplugin-grafana:${var.cpd_gf_image_tag}"
    }
  }

  storage_account {
    access_key   = azurerm_storage_account.gfsa.primary_access_key
    account_name = azurerm_storage_account.gfsa.name
    name         = azurerm_storage_account.gfsa.name
    share_name   = azurerm_storage_share.gffileshare.name
    type         = "AzureFiles"
    mount_path   = "/var/lib/grafana" # The directory Grafana uses for persistent data
  }

  logs {
    http_logs {
      file_system {
        retention_in_days = 31
        retention_in_mb   = 100
      }
    }
  }

  tags = data.azurerm_resource_group.rg.tags
}