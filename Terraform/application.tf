resource "azurerm_service_plan" "service-plan" {
  name                = var.service_plan_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = var.service_plan_sku[terraform.workspace]
  tags                = data.azurerm_resource_group.rg.tags
}

resource "azurerm_linux_web_app" "linux-web-app" {
  name                = var.web_app_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  service_plan_id     = azurerm_service_plan.service-plan.id

  app_settings = {
    CPD_GOOGLEANALYTICSTAG               = var.cpd_googleanalyticstag
    CPD_KEYVAULTENDPOINT                 = var.cpd_keyvaultendpoint
    CPD_CLIENTID                         = var.cpd_client_id
    CPD_CLIENTSECRET                     = var.cpd_client_secret
    CPD_SPACE_ID                         = var.cpd_space_id
    CPD_PREVIEW_KEY                      = var.cpd_preview_key
    CPD_DELIVERY_KEY                     = var.cpd_delivery_key
    CPD_TENANTID                         = var.tenant_id
    CPD_AZURE_ENVIRONMENT                = lower(terraform.workspace)
    CPD_CONTENTFUL_ENVIRONMENT           = var.cpd_contentful_env[terraform.workspace]
    CPD_INSTRUMENTATION_CONNECTIONSTRING = data.azurerm_application_insights.appinsights.connection_string
    DOCKER_ENABLE_CI                     = "true"
  }

  site_config {
    application_stack {
      docker_image     = "ghcr.io/dfe-digital/childrens-social-care-cpd"
      docker_image_tag = "master"
      # docker_image_tag = lower(terraform.workspace)
    }
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
