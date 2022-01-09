# MonsterCardGame Protocol
## Table of contents
* [Design](##design)
* [Lessons Learned](##Lessons Learned)
* [Unit Tests](##unit tests)
* [Unique Feature](##Unique Feature)

## Design
###Battle Logic
The first step was the UML diagramm for the battle logic, which we drew during class. I decided to create an abstract Class for Cards, so that common functions can be executed from within ever Cardtype.
The draft is atteched in the protcol directory, but the up to date class diagram is added in every subfolder of the project.
At first, I wanted to provide the specific implementation of this method in every child class, so that every Child class knows against which card it is not efficient. 
Unfortunately, this would cause dependencies to sub classes, which is against the oop principle and makes it harder to maintain the code. 
Therefore, I decided to create an own function in the abstract class for handling these special cases in a centralized manner.  
The current class diagramm of the card logic is illustrated with ClassDiagrammCards.cd in the cards directory.

###Server
Here the main class is the HTTP Server Class. Basically a TCP listener is started with this class and for every incoming request an thread is started to prepare further actions.
Whithin the StartClientHandling Method an Object from the Request and Response Class is instanciated. These Classes are meant to process the request header/ body and to generate a HTTP response from the server.
First the header is analyzed and the content saved as dictionary. Then this data and the response object are passed to the constructor of the Routing class.
In this class lays the logic for determining the appropriate endpoint. Depending on the path which was sent in the header a different Handler Object is created with certain actions for the request.
The Handler base class contains the mechanism for authorisation, useful function which can be shared among Handlers and the abstract Method handle(). This is the main method for for the business logic.
In the Child Classes the deserialization of the sent JSON takes place(one struct for every child class + Method dezerializeMessage). 
After the actions are worked off the server response is sent within the handle Method of the certain Handler, through the Response object. 

The Session Class is meant for including functions which dont belong exactly in other server classes (mostly because they are static and it would be confising addressing them with these other classes)
For example it keeps track of a concurrent dictionary which registers every login on the server with the corresponding token.
This feature is needed to vertify that a login took place before a certain token can be used => User altenhof needs to login, before any request with altenhof-token is accepted. 

###Database
I decided to use the DAO pattern for database interaction, because it was rather straight forward in comparison to the repository pattern and it seemed familiar from the WEB lessons in previus semesters.
The pattern is needed to provide a separation between business logic and persistance layer. So both layers are independent(= less work changing database for example).
Basically there is a class for the sql commands for the database (in the folder implementation) + their Interfaces and one class for the Model which is exhanged between layers.
So whenever the handle Function is called after an request, there are certain actions leading to creation/ receiving of Models. These models can be updated in the handler and be sent to the DAO implementation, to persist the data in the database.

An additional key concept is the database class. This class takes care of the connection to the database. 
The sigleton pattern is used in this case, to gurantee, that only one database connection object is created and always the same connection is used, for every sql statement. 
Although a better solution for handling the database connection could have been the usage of connection pools for the database. 
	
## Lessons Learned
*I spent much time on finding solutions for general coding problems, for example creating objects in an automated manner. While just checking common patterns, like the code factory could have done the trick.
*Write unit tests! => I created one small unit tests for one function of the fighting logic and realized through this tests, that i forgot the whole time about a break statement in a loop.
*Next time I should try to do the unit tests first + providing more Interfaces to create mocks. So that I adapt my code to fullfill the criteria and not the to 
*I should use dictionrys more often, to handle variables more efficiently 
*DAO Pattern: Business logic and model handling need to be strictly separated, therfore it is not allowed to mix these classes (e.g. cardModel and Handler Class of Card)
*Singleton Pattern is useful to create only one object of a class and share the same connection
*Be careful with asynch code => firstly I tried to listen to incoming tcpclients asynch and created afterwards the threads. 
*LINQ in combination with lambda expressions are really useful to handle lists => compressed code

## Unit Tests
Most of the Unit Tests were implemented for the battle logic, because the description made it very clear which features should be fullfilled. The battle logic is one of the main features of the whole project.
I designed unit tests to check the outcome of a battle round. For example if monster cards vs spell cards cosnider the effectifness of the element types and other specialities like Kraken against Spell cards.
Furthermore these tests helped a lot while changing the structure of the battle classes, because it made checking results easily achievable.

The database class is tested as well, for checking if connection can be established. This is especially usefull at the beginning of the project, to see if the connection even works.
I didnt want to implement further unit tests for the database because the integration tests where quite numerous.

The authorisation for users is critical too, because the user is not allowed to access every route on the server. 
The unit tests for this purpose check the outcome of the Authorization function of the Handler Class. 
This function knows if the user is already logged in on the server and decides if the request is blocked.
To test this function a mock was needed (because I use the Class Response in this function).
E.g.: User altenhof logged and therefore token saved on serverside. If user kienboec tries to send request with kienboec-Token, access needs to be denied. 
Or if the route is only for the admin users and a normal user tries to access it => the function should deny the request.

##Unique Feature
I emplemented a booster feature which is activated in random round. The purpose was to give the player which is most likely to lose (because lost more rounds until a certain moment) a booster.x
So after round 5 is played, the player with more loses is selected and gets, in one of the following 5 rounds, a random multiplyer for the calculated damage of the card.
The usage of the feature is restricted to only once in the whole game. 

Time spent: (tracked with toggl track)
95h

Github Link: 
https://github.com/mucki024/MonsterCardGame.git
