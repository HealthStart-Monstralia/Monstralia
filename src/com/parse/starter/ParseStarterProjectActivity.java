package com.parse.starter;

import android.app.Activity;
import android.os.Bundle;

import com.parse.GetCallback;
import com.parse.ParseAnalytics;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.SaveCallback;

import java.text.ParseException;

public class ParseStarterProjectActivity extends Activity {
    /** Called when the activity is first created. */
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);

        ColorQuiz cq = new ColorQuiz();
        Monster m = cq.getMonster();

        ParseAnalytics.trackAppOpenedInBackground(getIntent());

        DodgeGame dg = new DodgeGame(m);
        dg.run();
        MatchGame mtg = new MatchGame(m);
        mtg.run();
        MazeGame mzg = new MazeGame(m);
        mzg.run();

        ParseObject monster = new ParseObject("Monster");
        monster.put("name", m.getName());
        monster.put("color", m.getColor());
        monster.put("energyLevel", m.getEnergyLevel());

        monster.saveInBackground();
    }
}
