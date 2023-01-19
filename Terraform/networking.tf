resource "azurerm_virtual_network" "vnet1" {
  name                = var.vnet_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  address_space       = [var.vnet_address_space[terraform.workspace]]
  tags                = data.azurerm_resource_group.rg.tags

  lifecycle {
    prevent_destroy = true
  }
}

resource "azurerm_subnet" "frontend" {
  name                 = var.vnet_frontend_name[terraform.workspace]
  resource_group_name  = data.azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet1.name
  address_prefixes     = [var.vnet_frontend_prefixes[terraform.workspace]]
}

resource "azurerm_subnet" "backend" {
  name                                          = var.vnet_backend_name[terraform.workspace]
  resource_group_name                           = data.azurerm_resource_group.rg.name
  virtual_network_name                          = azurerm_virtual_network.vnet1.name
  address_prefixes                              = [var.vnet_backend_prefixes[terraform.workspace]]
  private_link_service_network_policies_enabled = false
}

data "azurerm_public_ip" "pip1" {
  name                = var.pip_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  # location            = data.azurerm_resource_group.rg.location
  # allocation_method   = "Static"
  # sku                 = "Standard"

  # lifecycle {
  #   prevent_destroy = true
  # }

  # tags = data.azurerm_resource_group.rg.tags
}

resource "azurerm_network_interface" "nic" {
  name                = var.nic_name[terraform.workspace]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name

  ip_configuration {
    name                          = var.network_nic_ip_conf_name[terraform.workspace]
    subnet_id                     = azurerm_subnet.backend.id
    private_ip_address_allocation = "Dynamic"
  }

  tags = data.azurerm_resource_group.rg.tags
}

# resource "azurerm_network_interface_application_gateway_backend_address_pool_association" "nic-assoc01" {
#   ip_configuration_name   = var.network_nic_ip_conf_name[terraform.workspace]
#   network_interface_id    = azurerm_network_interface.nic.id
#   backend_address_pool_id = tolist(azurerm_application_gateway.appgw.backend_address_pool).0.id
# }

resource "azurerm_subnet_network_security_group_association" "blockall" {
  subnet_id                 = azurerm_subnet.frontend.id
  network_security_group_id = azurerm_network_security_group.nsg.id
}
