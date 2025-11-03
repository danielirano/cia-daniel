using System;
using UnityEngine;

[Serializable]
public class Message {}

//inicio do jogo
[Serializable]
public class PlayGameMessage : Message
{
    public double time; 
    public int timeType;       // 0 - início, 1 - fim
    public int id_jogador;
    public int gameID;
    public int resourceID;

    public PlayGameMessage(double time, int timeType, int id_jogador, int gameID, int resourceID)
    {
        this.time = time;
        this.timeType = timeType;
        this.id_jogador = id_jogador;
        this.gameID = gameID;
        this.resourceID = resourceID;
    }
}

//Modo de jogo
[Serializable]
public class GameModeMessage : Message
{
    public double time;
    public int gameMode;       // 1 - livre, 2 - padrão, 3 - desafiador
    public int id_jogador;
    public int gameID;
    public int resourceID;

    public GameModeMessage(double time, int gameMode, int id_jogador, int gameID, int resourceID)
    {
        this.time = time;
        this.gameMode = gameMode;
        this.id_jogador = id_jogador;
        this.gameID = gameID;
        this.resourceID = resourceID;
    }
}

// Iniciar caso
[Serializable]
public class CaseSelectedMessage : Message
{
    public double time;
    public string timestats;   // exemplo: "iniciou o caso" - falta configurar
    public int id_jogador;
    public int gameID;
    public int resourceID;
    public int caseId;


    public CaseSelectedMessage(double time, string timestats, int id_jogador, int gameID, int resourceID, int caseId)
    {
        this.time = time;
        this.timestats = timestats;
        this.id_jogador = id_jogador;
        this.gameID = gameID;
        this.resourceID = resourceID;        
        this.caseId = caseId;
    }
}

// usar powerup
[Serializable]
public class PowerUpMessage : Message
{
    public int powerupType;    // powerup1, powerup2, powerup3, powerup4
    public double time;
    public string timestats;   // tempo em que usou o powerup
    public int id_jogador;
    public int gameID;
    public int resourceID;
    public bool powerup;       // true quando usar um powerup
    public int powerupUtilizado; // contador de usos

    public PowerUpMessage(int powerupType, double time, string timestats, int id_jogador, int gameID, int resourceID, bool powerup, int powerupUtilizado)
    {
        this.powerupType = powerupType;
        this.time = time;
        this.timestats = timestats;
        this.id_jogador = id_jogador;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.powerup = powerup;
        this.powerupUtilizado = powerupUtilizado;
    }
}

// acessar dados do caso
[Serializable]
public class CaseDetailsMessage : Message
{
    public double time;
    public string timestats;
    public int id_jogador;
    public int gameID;
    public int resourceID;
    public bool powerup;         // true quando usar "mostrar detalhes"
    public int detalhesUtilizado; // contador de usos

    public CaseDetailsMessage(double time, string timestats, int id_jogador, int gameID, int resourceID, bool powerup, int detalhesUtilizado)
    {
        this.time = time;
        this.timestats = timestats;
        this.id_jogador = id_jogador;
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
    public double time;
    public string timestats;
    public int id_jogador;
    public int gameID;
    public int resourceID;
    public bool palavraCorreta; // true = correta, false = incorreta

    public WordSendMessage(double time, string timestats, int id_jogador, int gameID, int resourceID, bool palavraCorreta)
    {
        this.time = time;
        this.timestats = timestats;
        this.id_jogador = id_jogador;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.palavraCorreta = palavraCorreta;
    }
}

// validacao no caca
[Serializable]
public class WordValidationMessage : Message
{
    public double time;
    public int id_jogador;
    public int gameID;
    public int resourceID;
    public string word;         // palavra enviada
    public bool correct;        // true = acertou no caça, false = errou no caça

    public WordValidationMessage (double time, int id_jogador, int gameID, int resourceID, string word, bool correct)
    {
        this.time = time;
        this.id_jogador = id_jogador;
        this.gameID = gameID;
        this.resourceID = resourceID;
        this.word = word;
        this.correct = correct;
    }
}
