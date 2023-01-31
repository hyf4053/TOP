import java.net.http.*;

http = HttpClient.newBuilder().build();

globals.echoText = text -> {
    http.send(HttpRequest.newBuilder(new URI("http://localhost:2019/"))
        .POST(HttpRequest.BodyPublishers.ofString('{ "version": 1, "id": 111, "type": "ExecuteCommand", "payload": { "command": "/e ' + text + '" } }')).build(),
        HttpResponse.BodyHandlers.ofString()    
    );
}