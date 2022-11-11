namespace Unit04.Game.Casting{
    public class FallingObject : Actor{
        private int pointVal;
        public FallingObject(int points){
            pointVal = points;
        }
        public int getPoints(){
            return pointVal;
        }
    }
}