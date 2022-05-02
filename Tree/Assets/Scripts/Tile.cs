using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
   public enum TileKind {
       Blank,
       Acorn,
       Grass
   }

   public bool isCovered;
   public Sprite coveredSprite;
   public TileKind tileKind = TileKind.Blank;
   private Sprite defaultSprite;
   private SpriteRenderer spriteyboi;
   

   private void Start(){
       spriteyboi = GetComponent<SpriteRenderer>();
       spriteyboi.sortingOrder = 1;
       spriteyboi.sortingLayerName = "foreground";

       defaultSprite = GetComponent<SpriteRenderer>().sprite;
       GetComponent<SpriteRenderer>().sprite = coveredSprite;
   }

   

   public void SetIsCovered(bool covered){
       isCovered = false;
       GetComponent<SpriteRenderer>().sprite = defaultSprite;
   }
}
