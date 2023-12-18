resource "azurerm_search_service" "ai-search" {
  name                = var.ai_search_service_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  sku                 = var.ai_search_service_sku[terraform.workspace]
  tags                = data.azurerm_resource_group.rg.tags
}
