using UnityEngine;


public class PlayerAppearance : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private Renderer _helmet;
  [SerializeField] private Renderer _helmetGlass;
  [SerializeField] private Renderer _body;
  [Space]
  [SerializeField] private Renderer _skiJet;
  #endregion


  #region Public Methods
  public void initRandom()
  {
    _helmet.material.color = Random.ColorHSV();
    _helmetGlass.material.color = Random.ColorHSV();

    _body.material.color = Random.ColorHSV();

    _skiJet.material.color = Random.ColorHSV();
  }
  #endregion
}