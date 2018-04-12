using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tuple<T1, T2> {

	public T1 first;
	public T2 second;

	public Tuple(T1 first, T2 second) {
		this.first = first;
		this.second = second;
	}

	public override string ToString () {
		return string.Format ("<{0}, {1}>", first, second);
	}

	public static bool operator ==(Tuple<T1, T2> tup1, Tuple<T1, T2> tup2) {
		return (tup1.first.Equals(tup2.first)) && (tup1.second.Equals(tup2.second));
	}

	public static bool operator !=(Tuple<T1, T2> tup1, Tuple<T1, T2> tup2) {
		return !(tup1 == tup2);
	}

    public override bool Equals (object obj) {
        if (ReferenceEquals (null, obj)) return false;
        if (ReferenceEquals (this, obj)) return true;
        if (obj.GetType () != this.GetType ()) return false;
        return Equals ((Tuple<T1, T2>)obj);
    }

    public override int GetHashCode () {
        int hash = 17;
        hash = hash * 23 + (first == null ? 0 : first.GetHashCode ());
        hash = hash * 23 + (second == null ? 0 : second.GetHashCode ());
        return hash;
    }
}
