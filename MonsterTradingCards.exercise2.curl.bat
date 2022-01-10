@echo off

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo CURL Testing for Monster Trading Cards Game
echo.
REM --------------------------------------------------
echo 2) Login Users
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.
echo.
echo.

REM --------------------------------------------------
echo 11) configure deck
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "[\"9e8238a4-8a7a-487f-9f7d-a8c97899eb48\", \"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\", \"951e886a-0fbf-425d-8df5-af2ee4830d85\", \"44c82fbc-ef6d-44ab-8c7a-9fb19a0e7c6e\"]"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[\"27051a20-8580-43ff-a473-e986b52f297a\", \"d04b736a-e874-4137-b191-638e0ff3b4e7\", \"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\"]"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo.
echo should fail and show original from before:
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[\"67f9048f-99b8-4ae4-b866-d8008d00c53d\", \"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"2272ba48-6662-404d-a9a1-41a9bed316d9\", \"b017ee50-1c14-44e2-bfd6-2c0c5653a37c\"]"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo.
echo should fail ... only 3 cards set
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[\"3871d45b-b630-4a0d-8bc6-a5fc56b6a043\", \"d04b736a-e874-4137-b191-638e0ff3b4e7\", \"aa9999a0-734c-49c6-8f4a-651864b14e62\"]"
echo.
echo.
echo should fail ... not users cards
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[\"9e8238a4-8a7a-487f-9f7d-a8c97899eb48\", \"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\", \"951e886a-0fbf-425d-8df5-af2ee4830d85\", \"44c82fbc-ef6d-44ab-8c7a-9fb19a0e7c6e\"]"
echo.
echo.

REM --------------------------------------------------
echo 12) show configured deck 
curl -X GET http://localhost:10001/deck --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo.
REM --------------------------------------------------
echo 13) show configured deck different representation
echo kienboec
curl -X GET http://localhost:10001/deck?format=plain --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo.
echo altenhof
curl -X GET http://localhost:10001/deck?format=plain --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 14) edit user data
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Basic altenhof-mtcgToken"
echo.
curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"Name\": \"Kienboeck\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo.
curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "{\"Name\": \"Altenhofer\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo.
echo should fail:
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Basic altenhof-mtcgToken"
echo.
curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "{\"Name\": \"Hoax\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo.
curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"Name\": \"Hoax\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo.
curl -X GET http://localhost:10001/users/someGuy  --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 15) stats
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 16) scoreboard
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 17) battle
start /b "kienboec battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic kienboec-mtcgToken" -d ""
start /b "altenhof battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic altenhof-mtcgToken" -d ""
ping localhost -n 10 >NUL 2>NUL
echo.
echo.
REM --------------------------------------------------
echo 18) Stats 
echo kienboec
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo altenhof
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 19) scoreboard
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo.


REM --------------------------------------------------
echo 20) trade
echo check trading deals
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo create trading deal
curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"fc305a7a-36f7-4d30-ad27-462ca0445649\", \"Type\": \"monster\", \"MinimumDamage\": 15}"
echo.
echo check trading deals
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo delete trading deals
curl -X DELETE http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo should fail: deck card for trade offer
curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"9e8238a4-8a7a-487f-9f7d-a8c97899eb48\", \"Type\": \"monster\", \"MinimumDamage\": 15}"
echo.
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo.
REM --------------------------------------------------
echo 21) check trading deals (Ork 40 kienboec)
curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"fc305a7a-36f7-4d30-ad27-462ca0445649\", \"Type\": \"monster\", \"MinimumDamage\": 15}"
echo.
echo check trading deals
curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/tradings  --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo try to trade with yourself (should fail)
curl -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "\"b2237eca-0271-43bd-87f6-b22f70d42ca4\""
echo.
echo try to trade, not fullfilling requirements (to weak monster)
curl -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "\"70962948-2bf7-44a9-9ded-8c68eeac7793\""
echo.
echo try to trade, not fullfilling requirements (Waterspell)
curl -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "\"e85e3976-7c86-4d06-9a80-641c2019a79f\""
echo.
echo.
echo try to trade (Knight 21 from altenhof)
curl -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "\"ce6bcaee-47e1-4011-a49e-5a4d7d4245f3\""
echo.
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/tradings --header "Authorization: Basic altenhof-mtcgToken"
echo.

REM --------------------------------------------------
echo end...

REM this is approx a sleep 
ping localhost -n 1000 >NUL 2>NUL
@echo on