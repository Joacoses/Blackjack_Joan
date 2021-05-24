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
    //para las apuestas
    public Button apuesta10;
    public Button Allin;
    public Button resta10;
    public Text apuestaMessage;
    public Text BancaMessage;
    int apuesta = 0;
    public int banca = 1000;

    public int[] values = new int[52];
    int cardIndex = 0;
    //-----------------------------------------------------

    //declaraciones
    ArrayList posCartasSacadas = new ArrayList();
    private int contadorHits = 1;

    //public Text saldo;

    private void Awake()
    {    
        InitCardValues();        

    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------


    private void Start()
    {
        ShuffleCards();
        StartGame();
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------


    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */

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

             Esto 4 veces
             -------------------------------------------------------*/

        int contadorPalo = 0;
        int valor = 1;

        for (int i = 0; i < faces.Length; i++)
        {
            //cuando llegue a 13, reinicia el valor
            if (contadorPalo == 13)
            {
                valor = 1;
                contadorPalo = 0;
            }
            //añade el valor y suma el contador
            values[i] = valor;
            valor++;
            contadorPalo++;
        }//for

        //valores para j,q,k y as
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


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */

        //num posicion aleatoria
        int posicionAleatoria = Random.Range(0, 51);


        //si es una carta que ya ha hecho, se llama a si mismo y la vuelve a hacer
        if (posCartasSacadas.Contains(posicionAleatoria))
        {
            ShuffleCards();
        }
        //pone las cartas que salen en la lista
        else
        {
            cardIndex = posicionAleatoria;
            posCartasSacadas.Add(posicionAleatoria);
        }
    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    void StartGame()
    {
        apuesta = 0;
        actualizarBanca();

        //si no te queda dinero no te deja apostar
        if (banca == 0)
        {
            finalMessage.text = "No te queda dinero";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = false;
        }
        //damos cartas al player y al dealer
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }

        //si el dealer consigue 21, pierdes
        if (dealer.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blackjack de primeras!   HAS PERDIDO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            dealer.GetComponent<CardHand>().InitialToggle();
            banca += 0;
            actualizarBanca();
        }

        //si el jugador consigue 21, ganas
        if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blackjack de primeras!   HAS GANADO";
            hitButton.interactable = false;
            stickButton.interactable = false;
            banca += apuesta * 2;
            actualizarBanca();
        }

        //si el dealer consigue 21, pierdes
        if (dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Empate";
            hitButton.interactable = false;
            stickButton.interactable = false;
            dealer.GetComponent<CardHand>().InitialToggle();
            banca += 0;
            actualizarBanca();
        }


    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    private void CalculateProbabilities()
    {
        /*TODO:
      * Calcular las probabilidades de:
      * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador    
      * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta                   
      * - Probabilidad de que el jugador obtenga más de 21 si pide una carta                             
      */
      /*
        if (player.GetComponent<CardHand>().points <= 11)
        {
            probMessage.text = "100% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 12)
        {
            probMessage.text = "91% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 13)
        {
            probMessage.text = "83% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 14)
        {
            probMessage.text = "76% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 15)
        {
            probMessage.text = "65% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 16)
        {
            probMessage.text = "59% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 17)
        {
            probMessage.text = "48% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 18)
        {
            probMessage.text = "42% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 19)
        {
            probMessage.text = "34% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 20)
        {
            probMessage.text = "18% de no pasarse";
        }
        if (player.GetComponent<CardHand>().points == 21)
        {
            probMessage.text = "0% de no pasarse";
        }*/

    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;
        ShuffleCards();
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------


    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        ShuffleCards();
        CalculateProbabilities();
    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------


    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        resta10.interactable = false;
        apuesta10.interactable = false;
        Allin.interactable = false;

        if (contadorHits == 1)
        {
            //dealer.GetComponent<CardHand>().InitialToggle();
            contadorHits++;
        }
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */


        //hit y nos pasamos de 21
        if (player.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "HAS PERDIDO (te has pasado)";
            hitButton.interactable = false;
            stickButton.interactable = false;
            banca += 0;
            actualizarBanca();
        }
        

    }



    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    public void Stand()
    {

        //botones desactivados
        resta10.interactable = false;
        apuesta10.interactable = false;
        Allin.interactable = false;
        hitButton.interactable = false;
        stickButton.interactable = false;


        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        if (contadorHits == 1)
        {
            dealer.GetComponent<CardHand>().InitialToggle();
            contadorHits++;
        }
        

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */




        //mientras el dealer tenga menos 16, este pedirá carta
        while (dealer.GetComponent<CardHand>().points <= 16) 
        { 
            PushDealer(); 
        }


        
        //dealer>player y dealer<=21
        if (dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points && dealer.GetComponent<CardHand>().points <=21)
        {
            finalMessage.text = "HAS PERDIDO";
            
            dealer.GetComponent<CardHand>().InitialToggle();
            banca += 0;
            actualizarBanca();
        }



        //player > dealer o dealer > 21
        if (player.GetComponent<CardHand>().points > dealer.GetComponent<CardHand>().points || dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "HAS GANADO";
            
            dealer.GetComponent<CardHand>().InitialToggle();
            banca += apuesta * 2;
            actualizarBanca();

        }



        //player == dealer
        if (player.GetComponent<CardHand>().points == dealer.GetComponent<CardHand>().points)
        {
            finalMessage.text = "EMPATE";
            
            dealer.GetComponent<CardHand>().InitialToggle();
            banca += apuesta;
            actualizarBanca();
        }

    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    public void PlayAgain()
    {
        //botones activos otra vez
        hitButton.interactable = true;
        stickButton.interactable = true;
        resta10.interactable = true;
        apuesta10.interactable = true;
        Allin.interactable = true;

        //mensaje transparente
        finalMessage.text = "";

        //quitamos las cartas de la mesa
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;

        //vacia
        posCartasSacadas.Clear();
        //hits a 1
        contadorHits = 1;

        ShuffleCards();
        StartGame();
    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    //restamos 10 a banca y se lo sumamos a apuesta
    public void apostar10()
    {
        if (banca > 10)
        {

            apuesta += 10;
            banca -= 10;
            actualizarBanca();
        }
    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------


    //restamos 10 a apuesta y se lo sumamos a banca
    public void restar10()
    {
        if (banca > 10)
        {

            apuesta -= 10;
            banca += 10;
            actualizarBanca();
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------


    public void allin()
    {
        if (banca > 10)
        {
            apuesta += banca;
            banca -= banca;
            actualizarBanca();
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------


    private void actualizarBanca() 
    {
        apuestaMessage.text = apuesta.ToString();
        BancaMessage.text = banca.ToString();
    }

}
