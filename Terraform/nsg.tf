resource "azurerm_network_security_group" "nsg" {
  name                = "s185d01-chidrens-social-care-cpd-sn01-nsg"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = azurerm_resource_group.rg.tags
}

resource "azurerm_network_security_rule" "nsg-rule-01" {
  name                        = "AllowAzureLoadBalancerInBound"
  priority                    = 3651
  direction                   = "Inbound"
  access                      = "Allow"
  protocol                    = "*"
  source_port_range           = "*"
  destination_port_range      = "65200-65535"
  source_address_prefix       = "*"
  destination_address_prefix  = "*" # azurerm_subnet.frontend.address_prefixes
  resource_group_name         = azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.nsg.name
}
