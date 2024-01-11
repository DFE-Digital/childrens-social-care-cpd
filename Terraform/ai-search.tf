# Populate a local to use if grafana is to be configured in Pre-Prod or Prod or Load-Test
locals {
  with_basic_aisearch_count = can(regex("^Prod|Load", terraform.workspace)) ? 1 : 0
  with_basic_aisearch_bool  = can(regex("^Prod|Load", terraform.workspace)) ? true : false
  create_aisearch_count     = can(regex("^Test|Prod", terraform.workspace)) ? 1 : 0
}

resource "azurerm_search_service" "ai-search" {
  count                         = local.create_aisearch_count
  name                          = var.ai_search_service_name[terraform.workspace]
  resource_group_name           = data.azurerm_resource_group.rg.name
  location                      = data.azurerm_resource_group.rg.location
  public_network_access_enabled = local.with_basic_aisearch_bool ? false : true
  sku                           = var.ai_search_service_sku[terraform.workspace]
  tags                          = data.azurerm_resource_group.rg.tags
}
