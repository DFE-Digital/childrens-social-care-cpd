Feature: Error handling
Scenario: When service runs into error 404 display the ‘page not found’ page
    Given a user has arrived on the home page
    When a service page has not been found
    Then the 'Page not found' error page is displayed

Scenario: When service is down the service down page displayed
    Given a user has arrived on the home page
    When a the service is down
    Then the 'Sorry, there is a problem with the service' error page is displayed