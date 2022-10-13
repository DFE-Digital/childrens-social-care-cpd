resource "azurerm_container_registry" "acr" {
  name                = "s185dcontainerregistry"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Basic"
  admin_enabled       = false

  tags = {
    "Environment"  = "Dev",
    "Portfolio"    = "Digital and Technology",
    "Service Line" = "Childrens Social Care Improvement and Learning",
    "Service"      = "Azure Platform"
  }
}
