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
        this.playerID = 1; // ToDO
        this.gameID = CarregaDados.conf.gameID;
        this.resourceID = CarregaDados.conf.resourceID;
        this.time = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

    }
}

//inicio do jogo
[Serializable]
public class PlayGameMessage : Message
{
    public PlayGameMessage()
    {
    }
}

//Modo de jogo
[Serializable]
public class GameModeMessage : Message
{
    public int gameMode;       // 1 - livre, 2 - padrão, 3 - desafiador
    
    public GameModeMessage(int gameMode)
    {
        this.gameMode = gameMode;
    }
}

// Iniciar caso
[Serializable]
public class CaseSelectedMessage : Message
{
    public int caseID;

    public CaseSelectedMessage(int caseID)
    {
        
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

    public PowerUpMessage(int powerupType, bool powerup, int powerupUtilizado)
    {
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

    public CaseDetailsMessage(bool powerup, int detalhesUtilizado)
    {
        this.powerup = powerup;
        this.detalhesUtilizado = detalhesUtilizado;
    }
}

// enviar palavra
[Serializable]
public class WordSendMessage : Message
{
    public bool palavraCorreta; // true = correta, false = incorreta

    public WordSendMessage(bool palavraCorreta)
    {
        this.palavraCorreta = palavraCorreta;
    }
}

// validacao no caca
[Serializable]
public class WordValidationMessage : Message
{
    public string word;         // palavra enviada
    public bool correct;        // true = acertou no caça, false = errou no caça

    public WordValidationMessage (string word, bool correct)
    {
        this.word = word;
        this.correct = correct;
    }
}
[Serializable]
public class TimeStatsMessage : Message
{
    public int timeEvent;  // 0 = jogo, 1 = fase/caso
    public int timeType;   // 0 = início, 1 = fim

    public TimeStatsMessage(int timeEvent, int timeType)
    {
        this.timeEvent = timeEvent;
        this.timeType = timeType;
    }
}

