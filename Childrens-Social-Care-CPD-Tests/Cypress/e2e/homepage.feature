Feature:  Home Page Tests
 Scenario: home page heading is 'Develop your career in child and family social work'
    Given a user has arrived on the home page
    Then the heading should say 'Develop your career in child and family social work'

 Scenario: home page sub heading is 'Find a programme by career stage'
    Given a user has arrived on the home page
    Then the sub heading should say 'Find a programme by career stage'


 Scenario: home page should have link to 'Practitioners'
    Given a user has arrived on the home page
    Then the link should exists to 'Practitioners'

 Scenario: home page should have link to 'Experienced practitioners'
    Given a user has arrived on the home page
    Then the link should exists to 'Experienced practitioners'

 Scenario: home page should have link to 'Senior managers'
    Given a user has arrived on the home page
    Then the link should exists to 'Senior managers'

 Scenario: home page should have link to 'Leaders'
    Given a user has arrived on the home page
    Then the link should exists to 'Leaders'

