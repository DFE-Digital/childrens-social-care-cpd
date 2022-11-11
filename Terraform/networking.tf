resource "azurerm_virtual_network" "vnet1" {
  name                = "s185d01-chidrens-social-care-cpd-vn01"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  address_space       = ["10.0.0.0/16"]
  tags                = azurerm_resource_group.rg.tags
}

resource "azurerm_subnet" "frontend" {
  name                 = "s185d01-chidrens-social-care-cpd-sn01"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet1.name
  address_prefixes     = ["10.0.0.0/26"]
}

resource "azurerm_subnet" "backend" {
  name                                          = "s185d01-chidrens-social-care-cpd-sn02"
  resource_group_name                           = azurerm_resource_group.rg.name
  virtual_network_name                          = azurerm_virtual_network.vnet1.name
  address_prefixes                              = ["10.0.0.128/26"]
  private_link_service_network_policies_enabled = false
}

resource "azurerm_public_ip" "pip1" {
  name                = "s185d01AGPublicIPAddress"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  allocation_method   = "Static"
  sku                 = "Standard"
  tags                = azurerm_resource_group.rg.tags
}

resource "azurerm_network_interface" "nic" {
  name                = "s185d01nic-1"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  ip_configuration {
    name                          = "s185d01nic-ipconfig-1"
    subnet_id                     = azurerm_subnet.backend.id
    private_ip_address_allocation = "Dynamic"
  }

  tags = azurerm_resource_group.rg.tags
}

resource "azurerm_network_interface_application_gateway_backend_address_pool_association" "nic-assoc01" {
  network_interface_id    = azurerm_network_interface.nic.id
  ip_configuration_name   = "s185d01nic-ipconfig-1"
  backend_address_pool_id = tolist(azurerm_application_gateway.appgw.backend_address_pool).0.id
}

resource "azurerm_subnet_network_security_group_association" "blockall" {
  subnet_id                 = azurerm_subnet.frontend.id
  network_security_group_id = azurerm_network_security_group.nsg.id
}
