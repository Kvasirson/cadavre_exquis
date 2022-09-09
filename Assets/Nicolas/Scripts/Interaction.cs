using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State{EMPTY, EGG, PART}

public class Interaction : MonoBehaviour
{
	GameManager _gameManager;

	private bool _isInRange;
	private State _state;

	private ShopScript _vendor;
	private EggScript _egg;

	PartObject _heldPart;


    private void Start()
    {
		_gameManager = GameManager.GetInstance;

		_isInRange = false;
		_vendor = null;
		_state = State.EMPTY;
    }

    private void Update()
	{
        if (Input.GetButtonDown("Interact"))
        {
			if (_isInRange)
			{
				switch (_state)
				{
					case State.EMPTY:
						if(_vendor != null && _vendor.DisplayedObject != null && _gameManager._gold > 0 && _gameManager._eggIscomplete != true)
                        {
							BuyPart();
                        }
						else if(_egg != null && !_egg.IsEmpty)
                        {
							HoldEgg();
                        }
						break;

					case State.EGG:
						if (_vendor != null)
						{
							SellEgg();
							
						}
						else if (_egg != null)
						{
							PutEggDown();
						}
						break;

					case State.PART:
						if (_vendor != null)
						{
							break;
						}
						else if (_egg != null)
						{
							UsePart();
						}
						break;

					default:
						break;
				}
			}
        }
	}

	public State state
	{
		get { return _state; }
		set
		{
			_state = value;
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
    {
		if (!_isInRange)
		{
			if (col.tag == "pnj")
			{
				_isInRange = true;
				_vendor = col.transform.GetComponent<ShopScript>();

				if (_state == State.EMPTY)
                {
					_vendor.ShowDisplayedObject(true);
				}
			}
			else if (col.tag == "egg")
            {
				_isInRange = true;
				_egg = col.transform.GetComponent<EggScript>();
            }
		}
    }

    public void OnTriggerExit2D(Collider2D col)
    {
		if(_vendor != null)
        {
			if (_state == State.EMPTY)
            {
				_vendor.ShowDisplayedObject(false);
			}

			_vendor = null;
		}

		if(_state != State.EGG)
        {
			_egg = null;
		}

		_isInRange = false;
	}

    #region Interaction Outcomes
    void HoldEgg()
    {
		_state = State.EGG;
	}

	void PutEggDown()
    {
		_state = State.EMPTY;
    }

	void SellEgg()
    {
		_state = State.EMPTY;
		_gameManager.Gold += _egg.SoldValue(_vendor.FavoriteAttribute);

		if(_gameManager._gold < 0)
        {
			_gameManager.GameEnd();
        }

		_gameManager.ResetCurPool();
		_egg.Reset();
		_egg = null;
	}

	void BuyPart()
    {
		_state = State.PART;
		_gameManager._gold += - _vendor.ObjectPrice;
		_heldPart = _vendor.DisplayedObject;
		_vendor.ShowDisplayedObject(false);
		_gameManager.RemoveObjet(_heldPart);
    }

	void UsePart()
    {
		_state = State.EMPTY;
		_egg.AddPart(_heldPart);
		_heldPart = null;
		_gameManager.IncreaseEggTimer();
	}
    #endregion
}
