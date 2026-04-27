using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    [Serializable]
    public class CardData
    {
        public int idMedalla;              // 1=ElRisco, 2=Crocodile-Jump, 3=Laguna Mortal, 4=Salvia Selvatica
        public string nombreJuego;         // Texto visible para el popup de bloqueo
        public Image medallaImage;         // Donde se pinta la medalla obtenida
        public TextMeshProUGUI puntosText; // Tótems (mejor puntaje del juego)
        public GameObject lockedOverlay;   // Capa negra semi-transparente para bloqueado

        // Sprites propios de cada juego (cada equipo asigna sus medallas)
        public Sprite spritePlatino;
        public Sprite spriteOro;
        public Sprite spritePlata;
        public Sprite spriteBronce;
    }

    [Header("API")]
    public APIManager apiManager;

    [Header("UI - Header")]
    public TextMeshProUGUI nombreUsuarioText;
    public TextMeshProUGUI totalTotemsText;
    public TextMeshProUGUI progresoText;   // "x/4"

    [Header("Cards (4 en orden de storyline)")]
    public CardData[] cards;

    [Header("Popup bloqueado")]
    public GameObject popupBloqueado;
    public TextMeshProUGUI popupBloqueadoText;

    private int idUsuario;
    private int totalTotems;
    private Dictionary<int, int> mejorRankPorJuego = new Dictionary<int, int>(); // idMedalla -> 1=Oro, 2=Plata, 3=Bronce
    private Dictionary<int, int> puntosPorJuego    = new Dictionary<int, int>(); // idMedalla -> mejor_puntos
    private bool puntajeRecibido;
    private bool medallasRecibidas;

    void Start()
    {
        idUsuario = PlayerPrefs.GetInt("id_usuario");
        nombreUsuarioText.text = PlayerPrefs.GetString("usuario");

        if (popupBloqueado != null) popupBloqueado.SetActive(false);

        apiManager.GetPuntaje(idUsuario, OnPuntajeRecibido);
        apiManager.GetMedallas(idUsuario, OnMedallasRecibidas);
    }

    // ─── CALLBACKS DE API ────────────────────────────────────────

    void OnPuntajeRecibido(List<APIManager.PuntajeItem> items)
    {
        totalTotems = 0;
        puntosPorJuego.Clear();
        foreach (var item in items)
        {
            totalTotems += item.mejor_puntos;
            puntosPorJuego[item.id_medalla] = item.mejor_puntos;
        }
        totalTotemsText.text = totalTotems.ToString();
        puntajeRecibido = true;
        TryRefreshCards();
    }

    void OnMedallasRecibidas(List<APIManager.MedallaUsuario> items)
    {
        mejorRankPorJuego.Clear();
        foreach (var m in items)
        {
            int idMedalla = MapNombreAId(m.medalla);
            int rank      = MapRankANumero(m.rank);
            if (idMedalla == 0 || rank == 0) continue;

            // Nos quedamos con el mejor rank (número más alto: 4=Platino, 3=Oro, 2=Plata, 1=Bronce)
            if (!mejorRankPorJuego.ContainsKey(idMedalla) || rank > mejorRankPorJuego[idMedalla])
            {
                mejorRankPorJuego[idMedalla] = rank;
            }
        }
        medallasRecibidas = true;
        TryRefreshCards();
    }

    // ─── PINTADO DE CARDS ────────────────────────────────────────

    void TryRefreshCards()
    {
        if (!puntajeRecibido || !medallasRecibidas) return;
        RefreshCards();
    }

    void RefreshCards()
    {
        int completados = 0;
        int previousId = 0; // 0 = primera card siempre desbloqueada

        foreach (var card in cards)
        {
            bool tieneMedalla = mejorRankPorJuego.ContainsKey(card.idMedalla);
            bool desbloqueado = (previousId == 0) || mejorRankPorJuego.ContainsKey(previousId);

            if (card.lockedOverlay != null)
                card.lockedOverlay.SetActive(!desbloqueado);

            if (desbloqueado)
            {
                if (tieneMedalla)
                {
                    int rank = mejorRankPorJuego[card.idMedalla];
                    card.medallaImage.sprite = rank == 4 ? card.spritePlatino
                                              : rank == 3 ? card.spriteOro
                                              : rank == 2 ? card.spritePlata
                                              :             card.spriteBronce;
                    card.medallaImage.enabled = true;
                    completados++;
                }
                else
                {
                    card.medallaImage.enabled = false;
                }

                int puntos = puntosPorJuego.ContainsKey(card.idMedalla) ? puntosPorJuego[card.idMedalla] : 0;
                card.puntosText.text = puntos > 0 ? puntos.ToString() : "—";
            }
            else
            {
                card.medallaImage.enabled = false;
                card.puntosText.text = "—";
            }

            previousId = card.idMedalla;
        }

        if (progresoText != null)
            progresoText.text = $"{completados}/{cards.Length}";
    }

    // ─── NAVEGACIÓN (mismos nombres que tenía antes para no romper el wiring del Inspector) ───

    public void GoToElRisco()         => IntentarIr(1, "MA_Home");
    public void GoToCrocodileJump()   => IntentarIr(2, "Juan_Menu");
    public void GoToMinigameLuis()    => IntentarIr(3, "MenuGame4");   // Laguna Mortal
    public void GoToMinigameMarcelo() => IntentarIr(4, "MainMenu");    // Salvia Selvatica

    private void IntentarIr(int idMedalla, string escena)
    {
        // La primera card siempre está desbloqueada
        if (idMedalla == 1)
        {
            SceneManager.LoadScene(escena);
            return;
        }

        // Verifica que TODOS los juegos anteriores tengan medalla
        for (int i = 1; i < idMedalla; i++)
        {
            if (!mejorRankPorJuego.ContainsKey(i))
            {
                string nombreAnterior = NombreDeJuegoPorId(i);
                if (popupBloqueado != null && popupBloqueadoText != null)
                {
                    popupBloqueadoText.text = $"Necesitas medalla mínima de Bronce en \"{nombreAnterior}\" para desbloquear este minijuego.";
                    popupBloqueado.SetActive(true);
                }
                return;
            }
        }

        SceneManager.LoadScene(escena);
    }

    public void CerrarPopupBloqueado()
    {
        if (popupBloqueado != null) popupBloqueado.SetActive(false);
    }

    // ─── HELPERS ────────────────────────────────────────────────

    private string NombreDeJuegoPorId(int idMedalla)
    {
        foreach (var c in cards) if (c.idMedalla == idMedalla) return c.nombreJuego;
        return "el juego anterior";
    }

    private int MapNombreAId(string nombre)
    {
        switch (nombre)
        {
            case "El Risco":         return 1;
            case "Crocodile-Jump":   return 2;
            case "Laguna Mortal":    return 3;
            case "Salvia Selvatica": return 4;
            default:                 return 0;
        }
    }

    private int MapRankANumero(string rank)
    {
        // Mayor número = mejor medalla (Platino > Oro > Plata > Bronce)
        switch (rank)
        {
            case "Platino": return 4;
            case "ORO":     return 3;
            case "PLATA":   return 2;
            case "BRONZE":  return 1;
            default:        return 0;
        }
    }
}
