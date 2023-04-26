Feature: Senior Manager Page Tests
 Scenario: Senior Managers heading is 'Programmes for senior manager'
   Given a user has clicked on 'Senior managers'
    Then the heading should say 'Programmes for senior managers'

 Scenario: Pathway 3: heads of service from senior manager Page link opens Successfully
    Given a user has clicked on 'Senior managers'
    When user clicks on 'Pathway 3: heads of service'
    Then the heading should say 'Pathway 3: heads of service'

 Scenario: Pathway 4: practice leaders from senior manager Page link opens Successfully
    Given a user has clicked on 'Senior managers'
    When user clicks on ' Pathway 4: practice leaders'
    Then the heading should say 'Pathway 4: practice leaders'