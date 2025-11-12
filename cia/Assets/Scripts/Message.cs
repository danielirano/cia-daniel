using System;
using UnityEngine;

[Serializable]
public class Message {

    public string messageType;
    public int playerID;
    public int gameID;
    public int resourceID;
    public long time;

    public Message() {
        this.messageType = this.GetType().ToString();
        this.time = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }
}

//inicio do jogo
[Serializable]
public class PlayGameMessage : Message
{

    
    public PlayGameMessage(int playerID, int gameID, int resourceID)
    {
        this.playerID = playerID;
        this.gameID = gameID;
        this.resourceID = resourceID;

    }
}

//Modo de jogo
[Serializable]
public class GameModeMessage : Message
{
    public int gameMode;       // 1 - livre, 2 - padrão, 3 - desafiador
    
    public GameModeMessage(int playerID, int gameID, int resourceID, int gameMode)
    {
        this.playerID = playerID;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.gameMode = gameMode;
    }
}

// Iniciar caso
[Serializable]
public class CaseSelectedMessage : Message
{
    public int caseID;

    public CaseSelectedMessage(int playerID, int gameID, int resourceID, int caseID)
    {
        
        this.playerID = playerID;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.caseID = caseID;
    }
}

// usar powerup
[Serializable]
public class PowerUpMessage : Message
{
    public int powerupType;    // powerup1, powerup2, powerup3, powerup4
    public bool powerup;       // true quando usar um powerup
    public int powerupUtilizado; // contador de usos

    public PowerUpMessage(int playerID, int gameID, int resourceID, int powerupType, bool powerup, int powerupUtilizado)
    {
        this.playerID = playerID;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.powerupType = powerupType;
        this.powerup = powerup;
        this.powerupUtilizado = powerupUtilizado;
    }
}

// acessar dados do caso
[Serializable]
public class CaseDetailsMessage : Message
{
    public bool powerup;         // true quando usar "mostrar detalhes"
    public int detalhesUtilizado; // contador de usos

    public CaseDetailsMessage(int playerID, int gameID, int resourceID, bool powerup, int detalhesUtilizado)
    {
        this.playerID = playerID;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.powerup = powerup;
        this.detalhesUtilizado = detalhesUtilizado;
    }
}

// enviar palavra
[Serializable]
public class WordSendMessage : Message
{
    public bool palavraCorreta; // true = correta, false = incorreta

    public WordSendMessage(int playerID, int gameID, int resourceID, bool palavraCorreta)
    {
        this.playerID = playerID;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.palavraCorreta = palavraCorreta;
    }
}

// validacao no caca
[Serializable]
public class WordValidationMessage : Message
{
    public int gameID;
    public int resourceID;
    public string word;         // palavra enviada
    public bool correct;        // true = acertou no caça, false = errou no caça

    public WordValidationMessage (int playerID, int gameID, int resourceID, string word, bool correct)
    {
        this.playerID = playerID;
        this.time = time;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.word = word;
        this.correct = correct;
    }
}
[Serializable]
public class TimeStatsMessage : Message
{
    public int timeEvent;  // 0 = jogo, 1 = fase/caso
    public int timeType;   // 0 = início, 1 = fim

    public TimeStatsMessage(int playerID, int gameID, int resourceID, int timeEvent, int timeType)
    {
        this.playerID = playerID;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.timeEvent = timeEvent;
        this.timeType = timeType;
    }
}

