resource "azurerm_virtual_network" "vnet1" {
  name                = var.vnet_name[terraform.workspace]
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  address_space       = [var.vnet_address_space[terraform.workspace]]
  tags                = azurerm_resource_group.rg.tags
}

resource "azurerm_public_ip" "pip1" {
  name                = var.pip_name[terraform.workspace]
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  allocation_method   = "Static"
  sku                 = "Standard"

  lifecycle {
    prevent_destroy = true
  }

  tags = azurerm_resource_group.rg.tags
}
