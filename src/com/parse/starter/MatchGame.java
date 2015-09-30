package com.parse.starter;

/**
 * Created by rainierababao on 6/7/15.
 */
public class MatchGame implements Game {

    Monster m;

    public MatchGame(Monster m) {
        this.m = m;
        System.out.println("MatchGame for" + m.getName() + "instantiated");
    }

    public void run() {
        while (m.getEnergyLevel() != 75) {
            int food = game();
            // One in three chance that the food is 0
            if (food == 0) {
                int oldEnergyLevel = m.getEnergyLevel();
                m.setEnergyLevel(oldEnergyLevel + 25);
            }
        }
    }

    public static int game() {
        int a = 0;
        int b = a + 1;
        while (a != b) {
            a = (int) (Math.random() * 4);
            b = (int) (Math.random() * 4);
        }
        return a;
    }

}
