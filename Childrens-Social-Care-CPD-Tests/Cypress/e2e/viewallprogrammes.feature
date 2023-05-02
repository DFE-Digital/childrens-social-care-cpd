Feature:  View All Programmes Tests
 Scenario: View all Programme heading 'Child and family social work development programmes'
    Given a user has clicked on 'Development programmes for social workers'
    Then the heading should say 'Child and family social work development programmes'

 Scenario: View all Programme sub heading 'Assessed and supported year in employment (ASYE)'
    Given a user has clicked on 'Development programmes for social workers'
    Then the sub heading should say 'Assessed and supported year in employment (ASYE)'

 Scenario: View all Programme sub heading 'Social Work Leadership Pathways programme'
    Given a user has clicked on 'Development programmes for social workers'
    Then the sub heading should say 'Social Work Leadership Pathways programme'

 Scenario: View all Programme sub heading 'Upon: Inspiring Leaders programme'
    Given a user has clicked on 'Development programmes for social workers'
    Then the sub heading should say 'Upon: Inspiring Leaders programme'

 Scenario: Assessed and supported year in employment link opens Successfully
    Given a user has clicked on 'Development programmes for social workers'
    When user clicks on ' Assessed and supported year in employment'
    Then the heading should say 'Assessed and supported year in employment (ASYE)'

 Scenario: A Pathway 1: practice supervisors link opens Successfully
    Given a user has clicked on 'Development programmes for social workers'
    When user clicks on 'Pathway 1: practice supervisors'
    Then the heading should say 'Pathway 1: practice supervisors'

 Scenario: Pathway 2: middle managers link opens Successfully
    Given a user has clicked on 'Development programmes for social workers'
    When user clicks on 'Pathway 2: middle managers'
    Then the heading should say 'Pathway 2: middle managers'

 Scenario: Pathway 3: heads of service link opens Successfully
    Given a user has clicked on 'Development programmes for social workers'
    When user clicks on 'Pathway 3: heads of service'
    Then the heading should say 'Pathway 3: heads of service'

 Scenario: Pathway 4: practice leaders link opens Successfully
    Given a user has clicked on 'Development programmes for social workers'
    When user clicks on ' Pathway 4: practice leaders'
    Then the heading should say 'Pathway 4: practice leaders'

 Scenario: Upon: aspirant directors programme link opens Successfully
    Given a user has clicked on 'Development programmes for social workers'
    When user clicks on 'Upon: aspirant directors programme'
    Then the heading should say 'Upon: aspirant directors programme'

 Scenario: Upon: new directors programme link opens Successfully
    Given a user has clicked on 'Development programmes for social workers'
    When user clicks on 'Upon: new directors programme'
    Then the heading should say 'Upon: new directors programme'