resource "azurerm_network_security_group" "nsg" {
  name                = "s185d01-chidrens-social-care-cpd-sn01-nsg"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  security_rule {
    name                       = "Allow-Al"
    priority                   = 500
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "*"
    source_address_prefix      = "86.10.229.100"
    destination_address_prefix = "*"
  }

  security_rule {
    name                       = "AllowVnetInBound"
    priority                   = 3650
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "*"
    source_address_prefix      = "VirtualNetwork"
    destination_address_prefix = "VirtualNetwork"
  }

  security_rule {
    name                       = "AllowAzureLoadBalancerInBound"
    priority                   = 3651
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "65200-65535"
    source_address_prefix      = "GatewayManager"
    destination_address_prefix = "*"
  }

  # security_rule {
  #   name                       = "DenyAllInBound"
  #   priority                   = 3655
  #   direction                  = "Inbound"
  #   access                     = "Deny"
  #   protocol                   = "Tcp"
  #   source_port_range          = "*"
  #   destination_port_range     = "*"
  #   source_address_prefix      = "*"
  #   destination_address_prefix = "*"
  # }

  # security_rule {
  #   name                       = "AllowVnetOutBound"
  #   priority                   = 3650
  #   direction                  = "Outbound"
  #   access                     = "Allow"
  #   protocol                   = "Tcp"
  #   source_port_range          = "*"
  #   destination_port_range     = "*"
  #   source_address_prefix      = "VirtualNetwork"
  #   destination_address_prefix = "VirtualNetwork"
  # }

  # security_rule {
  #   name                       = "AllowAzureInternetOutBound"
  #   priority                   = 3651
  #   direction                  = "Outbound"
  #   access                     = "Allow"
  #   protocol                   = "Tcp"
  #   source_port_range          = "*"
  #   destination_port_range     = "*"
  #   source_address_prefix      = "*"
  #   destination_address_prefix = "Internet"
  # }

  # security_rule {
  #   name                       = "DenyAllOutBound"
  #   priority                   = 3655
  #   direction                  = "Outbound"
  #   access                     = "Deny"
  #   protocol                   = "Tcp"
  #   source_port_range          = "*"
  #   destination_port_range     = "*"
  #   source_address_prefix      = "*"
  #   destination_address_prefix = "*"
  # }

  tags = azurerm_resource_group.rg.tags
}
