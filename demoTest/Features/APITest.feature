Feature: APITest

API Testing demo using HTTP Client

@ApiTest
Scenario Outline: Verify object details returned using ID
	Given User has a parameter <ID>
    When User requests object details using <Endpoint> and <ID>
    Then User should get object with name as <Name> and status code <StatusCode>

    Examples:
    |ID | Endpoint | StatusCode | Name | 
    |3  | objects  |  200       | Apple iPhone 12 Pro Max |
    |5  | objects  |  200       | Samsung Galaxy Z Fold2  |
    |1000  | objects  |  200    | Oppo  |

@ApiTest
Scenario Outline: Verify user is able to create new object
    
    Given User has new object parameters as <Color> and <capacityGB>
    When User creates object with endpoint <Endpoint> having <Color> and <capacityGB>
    Then User verifies object is created with color <Color>, capacity GB and status <StatusCode>
     Examples:
    | Endpoint | StatusCode | Color | capacityGB | 
    | objects  |  200       | Blue | 128          |
    | objects  |  400       | Grey | 256          |