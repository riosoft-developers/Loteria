//code witten by: Antonio Adame, 20237657



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectorController : MonoBehaviour {

    //outside scripts
    BoardCreatorController boardCreator;

    public RectTransform cardHolder;
    public RectTransform card;
    public RectTransform[] _cards;
    public RectTransform center;

    //selector arrows
    public RectTransform selectLeft, selectRight;

    public Transform cardSelected;

    private float distance;

    // used in selector to enlarge center card. 
    private bool newCenter;

    private float[] _distance;
    private float cardDist;
    private int minDist;
    private int currMin;
    private bool newMin;
    private bool doneSwiping = true;

    private float growByX, growByY;
    private float shrinkByX, shrinkByY;

    public int cardSelectedIndex;


	void Start () {
        boardCreator = GetComponent<BoardCreatorController>();

        growByX = _cards[0].transform.localScale.x * 2f;
        growByY = _cards[0].transform.localScale.y * 2f;

        shrinkByX = .15f;
        shrinkByY = .33f;


        int cardsLen = _cards.Length;
        _distance = new float[cardsLen];

        // find distance between cards. 
        distance = _cards[1].anchoredPosition.x - _cards[0].anchoredPosition.x;
	}
	
	
	void Update () {
        // disable selector arrows based on position. 
        if (minDist == 0)
        {
            selectLeft.gameObject.SetActive(false);
            selectRight.gameObject.SetActive(true);
        }
        else if (minDist == _cards.Length - 1)
        {
            selectLeft.gameObject.SetActive(true);
            selectRight.gameObject.SetActive(false);
        }
        else
        {
            selectLeft.gameObject.SetActive(true);
            selectRight.gameObject.SetActive(true);
        }



		// find distance of each card to the center. 
        for (int i = 0; i < _cards.Length; i++)
        {
            _distance[i] = Mathf.Abs(center.transform.position.x - _cards[i].transform.position.x);
        }
        // get smallest distance to center. 
        float min = Mathf.Min(_distance);

        // find closest card to center. 
        for (int j = 0; j < _cards.Length; j++)
        {
            if (min == _distance[j])
            {
                minDist = j;
                
            }
        }


        // go through cards and make sure to only increase size on selected card.
        for (int a = 0; a < _cards.Length; a++)
        {
            // select or diselect cards in selector panel according to which ones have been added to the board.
            if (boardCreator._alreadyPicked[boardCreator.currBoard, a])
                boardCreator.SelectCard(_cards[a]);
            else
                boardCreator.DeSelectCard(_cards[a]);


            // enlarge closest card to the center and minimize others.
            if (a == minDist)
            {
                cardSelectedIndex = a;
                cardSelected = _cards[a];

                _cards[a].transform.localScale = new Vector3(growByX, growByY, 1);
                _cards[a].SetSiblingIndex(_cards.Length);
            }
            else
            {
                _cards[a].transform.localScale = new Vector3(shrinkByX, shrinkByY, 1);

                
            }
        }
        
        
	}


    public void MoveTo(int cardNum)
    {
        
        distance = center.transform.position.x - _cards[cardNum].transform.position.x;
        StartCoroutine(moveUntilCenter(cardNum, distance, 6f));
        
    }

    // move until selected card is at the center.
    IEnumerator moveUntilCenter(int index, float dist, float speed)
    {

        float newX = 0.00f;
        if (dist < 0)
        {
            
            dist /= speed;
            while (center.transform.position.x - _cards[index].transform.position.x <= -0.01f)
            {
                newX = dist;
                cardHolder.transform.position += new Vector3(newX, 0, 0);

                yield return null;
            }
            if (center.transform.position.x - _cards[index].transform.position.x >= -0.01f)
            {
                doneSwiping = true;
            }
        }
        else if (dist > 0)
        {
            dist /= speed;
            while (center.transform.position.x - _cards[index].transform.position.x >= 0.01f)
            {
                newX = dist;
                cardHolder.transform.position += new Vector3(newX, 0, 0);

                yield return null;
            }

            if (center.transform.position.x - _cards[index].transform.position.x <= 0.01f)
            {
                doneSwiping = true;
            }
        }
    }

    public void SkipTen(string dir)
    {
        if (doneSwiping)
        {
            doneSwiping = false;
            if (dir == "right")
            {
                // skip 10 to the right. 
                if (minDist + 3 < _cards.Length)
                    StartCoroutine(moveUntilCenter(minDist + 3, center.transform.position.x - _cards[minDist + 3].transform.position.x, 10f));
                else
                    StartCoroutine(moveUntilCenter(_cards.Length - 1, center.transform.position.x - _cards[_cards.Length - 1].transform.position.x, 10f));
            }
            else if (dir == "left")
            {
                if (minDist - 3 >= 0)
                    StartCoroutine(moveUntilCenter(minDist - 3, center.transform.position.x - _cards[minDist - 3].transform.position.x, 10f));
                else
                    StartCoroutine(moveUntilCenter(0, center.transform.position.x - _cards[0].transform.position.x, 10f));
            }
        }
    }

    
}
