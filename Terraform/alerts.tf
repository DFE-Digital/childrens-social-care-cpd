data "azurerm_key_vault_secret" "email" {
  name         = "dfe-email-alert-address"
  key_vault_id = data.azurerm_key_vault.kv.id
}

resource "azurerm_monitor_action_group" "main" {
  name                = var.monitor_action_group_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  short_name          = var.monitor_action_group_shortname[terraform.workspace]

  email_receiver {
    name          = "DfE"
    email_address = data.azurerm_key_vault_secret.email.value
  }
}

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

resource "azurerm_monitor_scheduled_query_rules_alert_v2" "exception" {
  name                = "exception-alert"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name

  evaluation_frequency = "PT1M"
  window_duration      = "PT1M"
  scopes               = [azurerm_log_analytics_workspace.log-analytics-ws.id]
  severity             = 4

  criteria {
    query                   = <<-QUERY
        Perf
        | where TimeGenerated > ago(1h)
        | where CounterName == "% Processor Time" and InstanceName == "_Total" 
        | project TimeGenerated, Computer, CounterValue, _ResourceId
        | summarize AggregatedValue = avg(CounterValue)  by bin(TimeGenerated, 1h), Computer, _ResourceId   
      QUERY
    time_aggregation_method = "Count"
    threshold               = 1
    operator                = "GreaterThanOrEqual"

    failing_periods {
      minimum_failing_periods_to_trigger_alert = 1
      number_of_evaluation_periods             = 1
    }
  }

  auto_mitigation_enabled          = false
  workspace_alerts_storage_enabled = false
  description                      = "This is V2 custom log alert"
  display_name                     = "exception-alertv2"
  enabled                          = true
  query_time_range_override        = "P2D"
  skip_query_validation            = false

  action {
    action_groups = [azurerm_monitor_action_group.main.id]
  }

  tags = data.azurerm_resource_group.rg.tags
}
