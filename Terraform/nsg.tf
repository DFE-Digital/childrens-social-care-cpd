# Creation of the network security group
resource "azurerm_network_security_group" "nsg" {
  name                = var.nsg_name[terraform.workspace]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  tags                = data.azurerm_resource_group.rg.tags
}

# Rule required for the application gateway
resource "azurerm_network_security_rule" "nsg-rule-03" {
  name                        = "AllowAzureLoadBalancerInBound"
  priority                    = 3651
  direction                   = "Inbound"
  access                      = "Allow"
  protocol                    = "*"
  source_port_range           = "*"
  destination_port_range      = "65200-65535"
  source_address_prefix       = "*"
  destination_address_prefix  = "*" # azurerm_subnet.frontend.address_prefixes
  resource_group_name         = data.azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.nsg.name
}
