// using System.Collections.Generic;
// using System.Data;
// using System.Linq;
// using UnityEngine;

// public class XOManagement:MonoBehaviour{
//     private List<int[]>  winConditions= new List<int[]>();

//     public List<int[]> WinCondition(int tableSize, int lineLength){
//         for(int row = 0; row < tableSize; row++ ){
//             for(int startCol = 0; startCol < tableSize - lineLength + 1; startCol++){
//                 int[] condition = new int[lineLength];
//                 for(int i = 0; i < lineLength; i++){
//                     condition[i] = row * tableSize + startCol + i;
//                 }
//                 winConditions.Add(condition);
//             }
//         }
    

   
//         for(int col = 0; col < tableSize; col++ ){
//             for(int startRow = 0; startRow < tableSize - lineLength + 1; startRow++){
//                 int[] condition = new int[lineLength];
//                 for(int i = 0; i < lineLength; i++){
//                     condition[i] = (startRow+i)*5+col;
//                 }
//                 winConditions.Add(condition);
//             }
//         }
    

    
//         for (int row = 0; row <= tableSize - lineLength; row++) {
//             for (int col = 0; col <= tableSize - lineLength; col++) {
//                 int[] condition = new int[lineLength];
//                 for (int i = 0; i < lineLength; i++) {
//                     condition[i] = (row + i) * tableSize + col + i;
//                 }
//                 winConditions.Add(condition);
//             }
//         }

//         // Diagonal lines (Minor Diagonal)
//         for (int row = 0; row <= tableSize - lineLength; row++) {
//             for (int col = lineLength - 1; col < tableSize; col++) {
//                 int[] condition = new int[lineLength];
//                 for (int i = 0; i < lineLength; i++) {
//                     condition[i] = (row + i) * tableSize + col - i;
//                 }
//                 winConditions.Add(condition);
//             }
    
//     }
//     return winConditions;
//     }

//     public bool IsWon(Seed currPlayer, List<int[]> winConditions){
//         foreach (var condition in winConditions) {
//             if (condition.All(index => playerSeeds[index] == currPlayer)) {
//                 return true;
//             }
//         }
//         return false;
//     }
// }