using Microsoft.VisualBasic;using System;using System.Collections;using System.Collections.Generic;using System.Data;using System.Diagnostics;/// <summary>/// The AIMediumPlayer is a type of AIPlayer where it will try and destroy a ship/// if it has found a ship/// </summary>public class AIMediumPlayer : AIPlayer{	/// <summary>	/// Private enumarator for AI states. currently there are two states,	/// the AI can be searching for a ship, or if it has found a ship it will	/// target the same ship	/// </summary>	private enum AIStates	{		Searching,		TargetingShip	}	private AIStates _CurrentState = AIStates.Searching;	private Stack<Location> _Targets = new Stack<Location>();	public AIMediumPlayer(BattleShipsGame controller) : base(controller)	{	}	/// <summary>	/// GenerateCoordinates should generate random shooting coordinates	/// only when it has not found a ship, or has destroyed a ship and 	/// needs new shooting coordinates	/// </summary>	/// <param name="row">the generated row</param>	/// <param name="column">the generated column</param>	protected override void GenerateCoords(ref int row, ref int column)	{		do {			//check which state the AI is in and uppon that choose which coordinate generation			//method will be used.			switch (_CurrentState) {			case AIStates.Searching:				SearchCoords(ref row, ref column);				break;			case AIStates.TargetingShip:				TargetCoords(ref row, ref column);				break;			default:				throw new ApplicationException("AI has gone in an imvalid state");			}		} while ((row < 0 || column < 0 || row >= EnemyGrid.Height || column >= EnemyGrid.Width || EnemyGrid.Item(row, column) != TileView.Sea));		//while inside the grid and not a sea tile do the search	}	/// <summary>	/// TargetCoords is used when a ship has been hit and it will try and destroy	/// this ship	/// </summary>	/// <param name="row">row generated around the hit tile</param>	/// <param name="column">column generated around the hit tile</param>	private void TargetCoords(ref int row, ref int column)	{		Location l = _Targets.Pop();		if ((_Targets.Count == 0))			_CurrentState = AIStates.Searching;		row = l.Row;		column = l.Column;	}	/// <summary>	/// SearchCoords will randomly generate shots within the grid as long as its not hit that tile already	/// </summary>	/// <param name="row">the generated row</param>	/// <param name="column">the generated column</param>	private void SearchCoords(ref int row, ref int column)	{		row = _Random.Next(0, EnemyGrid.Height);		column = _Random.Next(0, EnemyGrid.Width);	}	/// <summary>	/// ProcessShot will be called uppon when a ship is found.	/// It will create a stack with targets it will try to hit. These targets	/// will be around the tile that has been hit.	/// </summary>	/// <param name="row">the row it needs to process</param>	/// <param name="col">the column it needs to process</param>	/// <param name="result">the result og the last shot (should be hit)</param>	protected override void ProcessShot(int row, int col, AttackResult result)	{		if (result.Value == ResultOfAttack.Hit) {			_CurrentState = AIStates.TargetingShip;			AddTarget(row - 1, col);			AddTarget(row, col - 1);			AddTarget(row + 1, col);			AddTarget(row, col + 1);		} else if (result.Value == ResultOfAttack.ShotAlready) {			throw new ApplicationException("Error in AI");		}	}	/// <summary>	/// AddTarget will add the targets it will shoot onto a stack	/// </summary>	/// <param name="row">the row of the targets location</param>	/// <param name="column">the column of the targets location</param>	private void AddTarget(int row, int column)	{		if (row >= 0 && column >= 0 && row < EnemyGrid.Height && column < EnemyGrid.Width && EnemyGrid.Item(row, column) == TileView.Sea) {			_Targets.Push(new Location(row, column));		}	}}
 u s i n g   S y s t e m . C o l l e c t i o n s . G e n e r i c ;  
 u s i n g   S y s t e m . D a t a ;  
 u s i n g   S y s t e m . D i a g n o s t i c s ;  
  
 / / /   < s u m m a r y >  
 / / /   T h e   A I M e d i u m P l a y e r   i s   a   t y p e   o f   A I P l a y e r   w h e r e   i t   w i l l   t r y   a n d   d e s t r o y   a   s h i p  
 / / /   i f   i t   h a s   f o u n d   a   s h i p  
 / / /   < / s u m m a r y >  
 p u b l i c   c l a s s   A I M e d i u m P l a y e r   :   A I P l a y e r  
 {  
 	 / / /   < s u m m a r y >  
 	 / / /   P r i v a t e   e n u m a r a t o r   f o r   A I   s t a t e s .   c u r r e n t l y   t h e r e   a r e   t w o   s t a t e s ,  
 	 / / /   t h e   A I   c a n   b e   s e a r c h i n g   f o r   a   s h i p ,   o r   i f   i t   h a s   f o u n d   a   s h i p   i t   w i l l  
 	 / / /   t a r g e t   t h e   s a m e   s h i p  
 	 / / /   < / s u m m a r y >  
 	 p r i v a t e   e n u m   A I S t a t e s  
 	 {  
 	 	 S e a r c h i n g ,  
 	 	 T a r g e t i n g S h i p  
 	 }  
  
 	 p r i v a t e   A I S t a t e s   _ C u r r e n t S t a t e   =   A I S t a t e s . S e a r c h i n g ;  
  
 	 p r i v a t e   S t a c k < L o c a t i o n >   _ T a r g e t s   =   n e w   S t a c k < L o c a t i o n > ( ) ;  
 	 p u b l i c   A I M e d i u m P l a y e r ( B a t t l e S h i p s G a m e   c o n t r o l l e r )   :   b a s e ( c o n t r o l l e r )  
 	 {  
 	 }  
  
 	 / / /   < s u m m a r y >  
 	 / / /   G e n e r a t e C o o r d i n a t e s   s h o u l d   g e n e r a t e   r a n d o m   s h o o t i n g   c o o r d i n a t e s  
 	 / / /   o n l y   w h e n   i t   h a s   n o t   f o u n d   a   s h i p ,   o r   h a s   d e s t r o y e d   a   s h i p   a n d    
 	 / / /   n e e d s   n e w   s h o o t i n g   c o o r d i n a t e s  
 	 / / /   < / s u m m a r y >  
 	 / / /   < p a r a m   n a m e = " r o w " > t h e   g e n e r a t e d   r o w < / p a r a m >  
 	 / / /   < p a r a m   n a m e = " c o l u m n " > t h e   g e n e r a t e d   c o l u m n < / p a r a m >  
 	 p r o t e c t e d   o v e r r i d e   v o i d   G e n e r a t e C o o r d s ( r e f   i n t   r o w ,   r e f   i n t   c o l u m n )  
 	 {  
 	 	 d o   {  
 	 	 	 / / c h e c k   w h i c h   s t a t e   t h e   A I   i s   i n   a n d   u p p o n   t h a t   c h o o s e   w h i c h   c o o r d i n a t e   g e n e r a t i o n  
 	 	 	 / / m e t h o d   w i l l   b e   u s e d .  
 	 	 	 s w i t c h   ( _ C u r r e n t S t a t e )   {  
 	 	 	 c a s e   A I S t a t e s . S e a r c h i n g :  
 	 	 	 	 S e a r c h C o o r d s ( r e f   r o w ,   r e f   c o l u m n ) ;  
 	 	 	 	 b r e a k ;  
 	 	 	 c a s e   A I S t a t e s . T a r g e t i n g S h i p :  
 	 	 	 	 T a r g e t C o o r d s ( r e f   r o w ,   r e f   c o l u m n ) ;  
 	 	 	 	 b r e a k ;  
 	 	 	 d e f a u l t :  
 	 	 	 	 t h r o w   n e w   A p p l i c a t i o n E x c e p t i o n ( " A I   h a s   g o n e   i n   a n   i m v a l i d   s t a t e " ) ;  
 	 	 	 }  
 	 	 }   w h i l e   ( ( r o w   <   0   | |   c o l u m n   <   0   | |   r o w   > =   E n e m y G r i d . H e i g h t   | |   c o l u m n   > =   E n e m y G r i d . W i d t h   | |   E n e m y G r i d . I t e m ( r o w ,   c o l u m n )   ! =   T i l e V i e w . S e a ) ) ;  
 	 	 / / w h i l e   i n s i d e   t h e   g r i d   a n d   n o t   a   s e a   t i l e   d o   t h e   s e a r c h  
 	 }  
  
 	 / / /   < s u m m a r y >  
 	 / / /   T a r g e t C o o r d s   i s   u s e d   w h e n   a   s h i p   h a s   b e e n   h i t   a n d   i t   w i l l   t r y   a n d   d e s t r o y  
 	 / / /   t h i s   s h i p  
 	 / / /   < / s u m m a r y >  
 	 / / /   < p a r a m   n a m e = " r o w " > r o w   g e n e r a t e d   a r o u n d   t h e   h i t   t i l e < / p a r a m >  
 	 / / /   < p a r a m   n a m e = " c o l u m n " > c o l u m n   g e n e r a t e d   a r o u n d   t h e   h i t   t i l e < / p a r a m >  
 	 p r i v a t e   v o i d   T a r g e t C o o r d s ( r e f   i n t   r o w ,   r e f   i n t   c o l u m n )  
 	 {  
 	 	 L o c a t i o n   l   =   _ T a r g e t s . P o p ( ) ;  
  
 	 	 i f   ( ( _ T a r g e t s . C o u n t   = =   0 ) )  
 	 	 	 _ C u r r e n t S t a t e   =   A I S t a t e s . S e a r c h i n g ;  
 	 	 r o w   =   l . R o w ;  
 	 	 c o l u m n   =   l . C o l u m n ;  
 	 }  
  
 	 / / /   < s u m m a r y >  
 	 / / /   S e a r c h C o o r d s   w i l l   r a n d o m l y   g e n e r a t e   s h o t s   w i t h i n   t h e   g r i d   a s   l o n g   a s   i t s   n o t   h i t   t h a t   t i l e   a l r e a d y  
 	 / / /   < / s u m m a r y >  
 	 / / /   < p a r a m   n a m e = " r o w " > t h e   g e n e r a t e d   r o w < / p a r a m >  
 	 / / /   < p a r a m   n a m e = " c o l u m n " > t h e   g e n e r a t e d   c o l u m n < / p a r a m >  
 	 p r i v a t e   v o i d   S e a r c h C o o r d s ( r e f   i n t   r o w ,   r e f   i n t   c o l u m n )  
 	 {  
 	 	 r o w   =   _ R a n d o m . N e x t ( 0 ,   E n e m y G r i d . H e i g h t ) ;  
 	 	 c o l u m n   =   _ R a n d o m . N e x t ( 0 ,   E n e m y G r i d . W i d t h ) ;  
 	 }  
  
 	 / / /   < s u m m a r y >  
 	 / / /   P r o c e s s S h o t   w i l l   b e   c a l l e d   u p p o n   w h e n   a   s h i p   i s   f o u n d .  
 	 / / /   I t   w i l l   c r e a t e   a   s t a c k   w i t h   t a r g e t s   i t   w i l l   t r y   t o   h i t .   T h e s e   t a r g e t s  
 	 / / /   w i l l   b e   a r o u n d   t h e   t i l e   t h a t   h a s   b e e n   h i t .  
 	 / / /   < / s u m m a r y >  
 	 / / /   < p a r a m   n a m e = " r o w " > t h e   r o w   i t   n e e d s   t o   p r o c e s s < / p a r a m >  
 	 / / /   < p a r a m   n a m e = " c o l " > t h e   c o l u m n   i t   n e e d s   t o   p r o c e s s < / p a r a m >  
 	 / / /   < p a r a m   n a m e = " r e s u l t " > t h e   r e s u l t   o g   t h e   l a s t   s h o t   ( s h o u l d   b e   h i t ) < / p a r a m >  
  
 	 p r o t e c t e d   o v e r r i d e   v o i d   P r o c e s s S h o t ( i n t   r o w ,   i n t   c o l ,   A t t a c k R e s u l t   r e s u l t )  
 	 {  
 	 	 i f   ( r e s u l t . V a l u e   = =   R e s u l t O f A t t a c k . H i t )   {  
 	 	 	 _ C u r r e n t S t a t e   =   A I S t a t e s . T a r g e t i n g S h i p ;  
 	 	 	 A d d T a r g e t ( r o w   -   1 ,   c o l ) ;  
 	 	 	 A d d T a r g e t ( r o w ,   c o l   -   1 ) ;  
 	 	 	 A d d T a r g e t ( r o w   +   1 ,   c o l ) ;  
 	 	 	 A d d T a r g e t ( r o w ,   c o l   +   1 ) ;  
 	 	 }   e l s e   i f   ( r e s u l t . V a l u e   = =   R e s u l t O f A t t a c k . S h o t A l r e a d y )   {  
 	 	 	 t h r o w   n e w   A p p l i c a t i o n E x c e p t i o n ( " E r r o r   i n   A I " ) ;  
 	 	 }  
 	 }  
  
 	 / / /   < s u m m a r y >  
 	 / / /   A d d T a r g e t   w i l l   a d d   t h e   t a r g e t s   i t   w i l l   s h o o t   o n t o   a   s t a c k  
 	 / / /   < / s u m m a r y >  
 	 / / /   < p a r a m   n a m e = " r o w " > t h e   r o w   o f   t h e   t a r g e t s   l o c a t i o n < / p a r a m >  
 	 / / /   < p a r a m   n a m e = " c o l u m n " > t h e   c o l u m n   o f   t h e   t a r g e t s   l o c a t i o n < / p a r a m >  
 	 p r i v a t e   v o i d   A d d T a r g e t ( i n t   r o w ,   i n t   c o l u m n )  
 	 {  
  
 	 	 i f   ( r o w   > =   0   & &   c o l u m n   > =   0   & &   r o w   <   E n e m y G r i d . H e i g h t   & &   c o l u m n   <   E n e m y G r i d . W i d t h   & &   E n e m y G r i d . I t e m ( r o w ,   c o l u m n )   = =   T i l e V i e w . S e a )   {  
 	 	 	 _ T a r g e t s . P u s h ( n e w   L o c a t i o n ( r o w ,   c o l u m n ) ) ;  
 	 	 }  
 	 }  
 }  
 