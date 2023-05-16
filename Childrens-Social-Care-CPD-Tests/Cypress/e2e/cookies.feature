Feature: User handles cookies
Scenario: cookies page url is '/?pageName=ViewCookies&pageType=PathwayDetails&sendingPage=HomePage&sendingPageType=Master'
    Given a user has arrived on the home page 
    When the 'btn-viewCookies' is selected
    Then the page URL ends with '?pageName=ViewCookies&pageType=PathwayDetails&sendingPage=HomePage&sendingPageType=Master'

Scenario: cookies page title is 'Cookies'
    Given a user has arrived on the home page 
    When the 'btn-viewCookies' is selected
    Then the page's title is 'Cookies'

Scenario:User Select View Cookies No Option is Selected
    Given a user has arrived on the home page 
    When the 'btn-viewCookies' is selected
    Then the no option is checked

Scenario:User Select Accept Cookies yes Option is Selected
    Given a user has arrived on the home page 
    When the 'btn-accept' is selected
    Then the cookies page  has 'btn-accepted' option is checked

Scenario:User Select Accept Cookies yes Option is Selected
    Given a user has arrived on the home page 
    When the 'btn-reject' is selected
    Then the cookies page  has 'btn-rejected' option is checked