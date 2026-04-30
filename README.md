Author : Eva Beaulieu

Lancement du projet:

1. ouvrir 2 session de terminal (tmux ou 2 tabs)

2. dans une session, cd dans tp2-Eva-afk/server
3. rouler: dotnet run --project server.csproj

4. dans l'autre session, cd dans tp2 tp2-Eva-afk/client
5. rouler: dotnet run --project client.csproj

server: 
- Commandes pour quitter: quit || exit

client:
- Commandes pour quitter (dans la reqête liée à l'url): quit || exit
- pour tester le code on commence par l'url, on fait enter, puis on entre la méthode
- effectuer en ordre les instructions suivantes pour simplifier les tests
- pour 201 : url: http://localhost:8088/api/messages ; méthode: post
- pour 200 : url: http://localhost:8088/api/messages ; méthode: get
- pour 200 : url: http://localhost:8088/api/messages/1 ; méthode: get
- pour 200 : url: http://localhost:8088/api/messages/1 ; méthode: put || patch
- pour 204 : url: http://localhost:8088/api/messages ; méthode: post
- pour 400 : url: http://localhost:8088/api/messages/14pagesDeTp ; méthode: post
- pour 404 : url: http://localhost:8088/api/messages/420 ; méthode: get
- pour 405 : url: http://localhost:8088/ ; méthode: post
- pour 500 : pas possible de le voir en temps normal si le serveur fait son travail correctement

Architecture :
- Client/Program.cs:
    - HandleClient()
        - Tape un url ou exit et une méthode
        - exit => ferme le client proprement
        - Parse l'url pour récupérer et stocker le port et host
        - Crée un tcpClient et un stream et se connecte au serveur pour envoyer une requête et afficher la réponse recu
    - SendRequest()
        - Crée un objet HttpRequest pour préserver un standard entre serveur et client
        - Formate le tout en json pour la communication en standard http/1.1
        - Envoi en async dans le stream la requête formatée en bytes
    - ReceiveResponse()
        - Recoit et affiche la réponse du serveur

- Server/Program.cs:
    - Main()
        - Part l'écoute sur le port 8088 et appel au code de gestion des requêtes
    - HandleServer()
        - Accepte une connexion par tcp et lit la requête
        - Traite la requête, génère une réponse et l'envoi au client
        - vide le flux tcp et ferme le client et le stream
        
- Server/Router:
    - Handle()
        - Détermine le  path et la méthode utilisée et retourne une réponse adaptée à ce que la requête veux/ peut recevoir
        - Utilise httpResponseWriter pour retourner la réponse bien formatée
        - Gère les erreurs en renvoyant des réponses d'erreur selon le type d'erreur trouvé
    - GetIdFromRequest()
        - Retourne l'id pour le requêtes qui ont un id (parse la requête)
    - VerifyIdContainedInMessages()
        - Oberve et répond avec vrai ou faux selon si l'id du message est présent dans le dictionnaire
        
- Shared/HtmlFileProvider:
    - SendHtmlPage()
        - lit le contenu de page.html
        - Retourne le contenu de la page html page.html en string
        
- Shared/HttpRequest:
    - Format standardisé du HttpRequest pour avoir une base commune entre le serveur et le client
    
- Shared/HttpResponse:
    - Format standardisé du HttpResponse pour avoir une base commune entre le serveur et le client

- Shared/HttpRequestParser:
    - ParseRequest()
            - Parse la requête en json et retourne un objet HttpRequest
            
- Shared/HttpRequestWriter:
    - ToBytes()
        - Recoit un HttpRequest et le converti en contenu bytes qu'on peut envoyer dans le stream en suivant le standard de Http/1.1
        
- Shared/HttpResponseWriter:
    - ToBytes()
        - Recoit un HttpResponse et le converti en contenu bytes qu'on peut envoyer dans le stream en suivant le standard de Http/1.1
        
- Données: 
    - On conserve dans le serveur les messages
    - Stockage en mémoire avec un Dictionnaire messages ayant une clé int et un contenu string
- Limites:
    - Support limité de formats de contenu (nombre de route limitées)
    - Performance non optimisée pour un usage en entreprise
    - Aucune connexion persistante supportée (close fréquents)
    - Pas une gestion complète du corps des requêtes possibles
    - N'est pas un serveur http complet
    - En bref, le vrai protocole Http/1.1 est bien plus vaste que ce petit projet d'une personne (petit mais très éducatif)
    
    

