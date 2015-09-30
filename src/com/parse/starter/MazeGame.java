package com.parse.starter;

import java.util.Arrays;

public class MazeGame implements Game {

    // position of monster
    private int position;
    // energy level of monster
    private int energyLevel;

    Monster m;
    static char[][] mazeMap = new char[5][5];

    public MazeGame(Monster m) {
        this.m = m;
        m.setPositionX(0);
        m.setPositionY(0);
        buildMaze();
        System.out.println("MazeGame for" + m.getName() + "instantiated");
    }

    public void run() {
        buildMaze();
    }

    public void buildMaze(){
        for(char[] row : mazeMap) {
            Arrays.fill(row, '*');
        }
        Food[] consumeFood = {new Food("banana", 25, true), new Food("corn", 25, true), new Food("cupcake", 25, false), new Food("berries", 25, true), new Food("bread", 25, true), new Food("candy", 25, false)};
        for (int i = 0; i < consumeFood.length; i++) {
            mazeMap[consumeFood[i].getPositionX()][consumeFood[i].getPositionY()] = 'F';
        }
        mazeMap[m.getPositionX()][m.getPositionY()] = 'M';
        for (int row = 0; row < mazeMap.length; row++) {
            for (int col = 0; col < mazeMap.length; col++) {
                System.out.print(mazeMap[row][col]);
            }
            System.out.println();
        }

        while (m.getEnergyLevel() < 100) {
            // move the monster
            int direction = (int) (Math.random() * 3);
            //System.out.println(direction);
            //System.out.println("before position: " + curr.getPositionX());
            if (m.getPositionX()+1 < mazeMap.length && m.getPositionY()+1 < mazeMap.length) {

                if (direction == 0) {
                    //System.out.print("CHANGE");
                    m.setPositionX((m.getPositionX()+1));
                    m.setPositionX(m.getPositionY()+1);
                }
                else if (direction == 1) {
                    m.setPositionX(m.getPositionY()+1);
                }
                else  {
                    m.setPositionX(m.getPositionX()+1);

                }
            }
            //System.out.println("after position: " + curr.getPositionX());


            if (mazeMap[m.getPositionX()][m.getPositionY()] == 'F') {
                // hard coded for now
                //curr.setEnergyLevel(curr.getEnergyLevel()+25);

                //System.out.print("You rock!");
                // this is gacky code - sorry =(   need to finish it quickly though
                for (int i = 0; i < consumeFood.length; i++) {
                    //System.out.println("FOOD: X" + consumeFood[i].getPositionX() + "Y " + consumeFood[i].getPositionY());
                    //System.out.println("MONSTER:" + curr.getPositionX()+ "Y " + curr.getPositionY());
                    // shoudl also check for y position but too random so might
                    if (consumeFood[i].getPositionX() == m.getPositionX()) {
                        if (consumeFood[i].getHealth()) {
                            m.setEnergyLevel(m.getEnergyLevel()+25);
                        }
                    }

                }
            }
        }

    }


}
