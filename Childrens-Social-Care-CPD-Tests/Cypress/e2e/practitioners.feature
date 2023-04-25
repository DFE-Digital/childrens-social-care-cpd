Feature:  Practitioners Link Tests
 Scenario: practitoners page heading is 'Programmes for practitioners'
    Given a user has arrived on the home page and click Practitioners link
    Then the heading should say 'Programmes for practitioners'

 Scenario: practitoners page has link to ' Assessed and supported year in employment'
    Given a user has arrived on the home page and click Practitioners link
    Then the link should exists to ' Assessed and supported year in employment'

 Scenario: practitoners page sub heading is 'Professional development for practitionerse'
    Given a user has arrived on the home page and click Practitioners link
     Then the sub heading should say 'Professional development for practitioners'