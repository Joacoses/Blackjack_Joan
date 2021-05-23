using UnityEngine;
using UnityEngine.UI;


//para el array
using System.Collections;


public class Deck : MonoBehaviour
{
    //-----------------------------------------------------
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;


    public int[] values = new int[52];
    int cardIndex = 0;
    //-----------------------------------------------------

    //declaraciones
    ArrayList posCartasSacadas = new ArrayList();
    private int contadorHits = 1;
    public int banca = 1000;
    public Text saldo;

    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();
        //saldo
        saldo.text = banca.ToString();
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

        // 11 2 3 4 5 6 7 8 9 10 10 10 10
        /*-------------------------------------------------------
            // 11 2 3 4 5 6 7 8 9 10 10 10 10
         
            //-----------------------------------------------------
             values[0] = 11;
             values[1] = 2;
             values[2] = 3;
             values[3] = 4;
             values[4] = 5;
             values[5] = 6;
             values[6] = 7;
             values[7] = 8;
             values[8] = 9;
             values[9] = 10;
             values[10] = 10;
             values[11] = 10;
             values[12] = 10;
            //-----------------------------------------------------

             Esto x4;
             -------------------------------------------------------*/

        int contadorPalo = 0;
        int valor = 1;

        for (int i = 0; i < faces.Length; i++)
        {
            if (contadorPalo == 13)
            {
                valor = 1;
                contadorPalo = 0;
            }
            values[i] = valor;
            valor++;
            contadorPalo++;
        }//for

        for (int j = 0; j < faces.Length; j++)
        {
            if (values[j] == 11 || values[j] == 12 || values[j] == 13)
            {
                values[j] = 10;
            }
            if (values[j] == 1)
            {
                values[j] = 11;
            }

        }//for

    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */

        int posicionAleatoria = Random.Range(0, 51);


        if (posCartasSacadas.Contains(posicionAleatoria))
        {
            ShuffleCards();
        }
        else
        {
            cardIndex = posicionAleatoria;
            posCartasSacadas.Add(posicionAleatoria);
        }
    }

    void StartGame()
    {

        banca = banca - 10;

        if (banca == 0)
        {
            finalMessage.text = "No te queda dinero";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = false;
        }

        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
        if (dealer.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blackjack!   HAS PERDIDO";
            hitButton.interactable = false;
            stickButton.interactable = false;
        }
        if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blackjack!   HAS GANADO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            banca = banca + 20;
            saldo.text = banca.ToString();
        }
        // Debug.Log("Puntos jugador: " + player.GetComponent<CardHand>().points);
        // Debug.Log("Puntos dealer: " + dealer.GetComponent<CardHand>().points);

    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
