# Setup an alert to email using the contact address in keyvault
data "azurerm_key_vault_secret" "email" {
  name         = "dfe-email-alert-address"
  key_vault_id = data.azurerm_key_vault.kv.id
}

# This is the main action group that sends an email
resource "azurerm_monitor_action_group" "main" {
  name                = var.monitor_action_group_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  short_name          = var.monitor_action_group_shortname[terraform.workspace]

  email_receiver {
    name          = "DfE"
    email_address = data.azurerm_key_vault_secret.email.value
  }

  tags = data.azurerm_resource_group.rg.tags
}

# An alert for the backend connection time 
resource "azurerm_monitor_metric_alert" "appgw-backend-connect-time" {
  name                = var.alert_appgw_backend_connect_time[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  scopes              = [azurerm_application_gateway.appgw.id]
  description         = "Action will be triggered when connect time is greater than 1000ms"
  window_size         = "PT5M"
  frequency           = "PT1M"

  criteria {
    metric_namespace = "Microsoft.Network/applicationGateways"
    metric_name      = "BackendConnectTime"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 1000
  }

  action {
    action_group_id = azurerm_monitor_action_group.main.id
  }

  tags = data.azurerm_resource_group.rg.tags
}

# An alert for the application gateway health
resource "azurerm_monitor_activity_log_alert" "appgw-health" {
  name                = var.alert_appgw_health[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  scopes              = [azurerm_application_gateway.appgw.id]
  description         = "Action will be triggered when backend health is bad"

  criteria {
    category = "ResourceHealth"

    resource_health {
      current  = ["Degraded", "Unavailable"]
      previous = ["Available"]
      reason   = ["PlatformInitiated", "Unknown"]
    }
  }

  action {
    action_group_id = azurerm_monitor_action_group.main.id
  }

  tags = data.azurerm_resource_group.rg.tags
}

# An alert that is raised with cpu usage is high
resource "azurerm_monitor_metric_alert" "container-cpu" {
  name                = var.alert_container_cpu[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  scopes              = [azurerm_service_plan.service-plan.id]
  description         = "Action will be triggered when CPU percentage is greater than 50%"
  window_size         = "PT5M"
  frequency           = "PT1M"

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "CpuPercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 50
  }

  action {
    action_group_id = azurerm_monitor_action_group.main.id
  }

  tags = data.azurerm_resource_group.rg.tags
}

# An alert when the average response time is high
resource "azurerm_monitor_metric_alert" "container-avg-resp-time" {
  name                = var.alert_container_avg_resp_time[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  scopes              = [azurerm_linux_web_app.linux-web-app.id]
  description         = "Action will be triggered when container average response time is greater than 1000ms"
  window_size         = "PT5M"
  frequency           = "PT1M"

  criteria {
    metric_namespace = "Microsoft.Web/sites"
    metric_name      = "AverageResponseTime"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 1000
  }

  action {
    action_group_id = azurerm_monitor_action_group.main.id
  }

  tags = data.azurerm_resource_group.rg.tags
}

# An alert for failed requests
resource "azurerm_monitor_metric_alert" "failed-requests" {
  name                = var.alert_failed_requests[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  scopes              = [data.azurerm_application_insights.appinsights.id]
  description         = "Action will be triggered when failed requests is greater than 1"
  window_size         = "PT5M"
  frequency           = "PT1M"

  criteria {
    metric_namespace = "microsoft.insights/components"
    metric_name      = "requests/failed"
    aggregation      = "Count"
    operator         = "GreaterThan"
    threshold        = 1
  }

  action {
    action_group_id = azurerm_monitor_action_group.main.id
  }

  tags = data.azurerm_resource_group.rg.tags
}