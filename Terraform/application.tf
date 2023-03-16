# Definition of a service plan for a Linux Container
resource "azurerm_service_plan" "service-plan" {
  name                = var.service_plan_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = var.service_plan_sku[terraform.workspace]
  tags                = data.azurerm_resource_group.rg.tags
}

# Definition of the linux web app for the service
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
    CPD_CLARITY                          = var.cpd_clarity
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

resource "azurerm_linux_web_app_slot" "staging" {
  name           = "staging"
  app_service_id = azurerm_linux_web_app.linux-web-app.id

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
    CPD_CLARITY                          = var.cpd_clarity
    DOCKER_ENABLE_CI                     = "true"
  }

  site_config {
    application_stack {
      docker_image     = "ghcr.io/dfe-digital/childrens-social-care-cpd"
      docker_image_tag = "master"
      # docker_image_tag = lower(terraform.workspace)
    }

    ip_restriction {
      name       = "Deny All"
      action     = "Deny"
      priority   = "1"
      ip_address = "0.0.0.0/0"
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

# The autoscaling settings for Load-Test and Prod environments
resource "azurerm_monitor_autoscale_setting" "autoscale" {
  name                = var.autoscale_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  target_resource_id  = azurerm_service_plan.service-plan.id

  count = terraform.workspace == "Prod" || terraform.workspace == "Load-Test" ? 1 : 0

  profile {
    name = "defaultProfile"

    capacity {
      default = 3
      minimum = 3
      maximum = 10
    }

    rule {
      metric_trigger {
        metric_name        = "CpuPercentage"
        metric_resource_id = azurerm_service_plan.service-plan.id
        time_grain         = "PT1M"
        statistic          = "Average"
        time_window        = "PT5M"
        time_aggregation   = "Average"
        operator           = "GreaterThan"
        threshold          = 70
      }

      scale_action {
        direction = "Increase"
        type      = "ChangeCount"
        value     = "1"
        cooldown  = "PT1M"
      }
    }

    rule {
      metric_trigger {
        metric_name        = "CpuPercentage"
        metric_resource_id = azurerm_service_plan.service-plan.id
        time_grain         = "PT1M"
        statistic          = "Average"
        time_window        = "PT5M"
        time_aggregation   = "Average"
        operator           = "LessThan"
        threshold          = 20
      }

      scale_action {
        direction = "Decrease"
        type      = "ChangeCount"
        value     = "1"
        cooldown  = "PT1M"
      }
    }
  }
}
