Feature: Managers Page Tests
 Scenario: Managers heading is 'Programmes for managers'
   Given a user has clicked on 'Managers'
    Then the heading should say 'Programmes for managers'

 Scenario: Pathway 2: middle managers from Managers Page link opens Successfully
    Given a user has clicked on 'Managers'
    When user clicks on 'Pathway 2: middle managers'
    Then the heading should say 'Pathway 2: middle managers'

 Scenario: Pathway 3: heads of service from Managers Page link opens Successfully
    Given a user has clicked on 'Managers'
    When user clicks on 'Pathway 3: heads of service'
    Then the heading should say 'Pathway 3: heads of service'