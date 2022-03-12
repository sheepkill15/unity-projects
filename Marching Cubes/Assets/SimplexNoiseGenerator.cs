using UnityEngine;

public class SimplexNoiseGenerator {
    private int[] _a = new int[3];
    private float _s, _u, _v, _w;
    private int _i, _j, _k;
    private float _onethird = 0.333333333f;
    private float _onesixth = 0.166666667f;
    private int[] _;
	
	public SimplexNoiseGenerator() {
        if (_ == null) {
            System.Random rand = new System.Random();
            _ = new int[8];
            for (int q = 0; q < 8; q++)
                _[q] = rand.Next();
        }
	}
	
	public SimplexNoiseGenerator(string seed) {
		_ = new int[8];
		string[] seedParts = seed.Split(new char[] {' '});
		
		for(int q = 0; q < 8; q++) {
			int b;
			try {
				b = int.Parse(seedParts[q]);
			} catch {
				b = 0x0;
			}
			_[q] = b;
		}
	}
	
	public SimplexNoiseGenerator(int[] seed) { // {0x16, 0x38, 0x32, 0x2c, 0x0d, 0x13, 0x07, 0x2a}
		_ = seed;
	}
	
	public string GetSeed() {
		string seed = "";
		
		for(int q=0; q < 8; q++) {
			seed += _[q].ToString();
			if(q < 7)
				seed += " ";
		}
		
		return seed;
	}
	
	public float CoherentNoise(float x, float y, float z, int octaves=1, int multiplier = 25, float amplitude = 0.5f, float lacunarity = 2, float persistence = 0.9f) {
		Vector3 v3 = new Vector3(x,y,z)/multiplier;
		float val = 0;
		for (int n = 0; n < octaves; n++) {
		  val += Noise(v3.x,v3.y,v3.z) * amplitude;
		  v3 *= lacunarity;
		  amplitude *= persistence;
		}
		return val;
	}
	
    public int GETDensity(Vector3 loc) {
		float val = CoherentNoise(loc.x, loc.y, loc.z);
		return (int)Mathf.Lerp(0,255,val);
    }
    
    // Simplex Noise Generator
    public float Noise(float x, float y, float z) {
        _s = (x + y + z) * _onethird;
        _i = Fastfloor(x + _s);
        _j = Fastfloor(y + _s);
        _k = Fastfloor(z + _s);

        _s = (_i + _j + _k) * _onesixth;
        _u = x - _i + _s;
        _v = y - _j + _s;
        _w = z - _k + _s;

        _a[0] = 0; _a[1] = 0; _a[2] = 0;

        int hi = _u >= _w ? _u >= _v ? 0 : 1 : _v >= _w ? 1 : 2;
        int lo = _u < _w ? _u < _v ? 0 : 1 : _v < _w ? 1 : 2;

        return Kay(hi) + Kay(3 - hi - lo) + Kay(lo) + Kay(0);
    }

    float Kay(int a) {
        _s = (_a[0] + _a[1] + _a[2]) * _onesixth;
        float x = _u - _a[0] + _s;
        float y = _v - _a[1] + _s;
        float z = _w - _a[2] + _s;
        float t = 0.6f - x * x - y * y - z * z;
        int h = Shuffle(_i + _a[0], _j + _a[1], _k + _a[2]);
        _a[a]++;
        if (t < 0) return 0;
        int b5 = h >> 5 & 1;
        int b4 = h >> 4 & 1;
        int b3 = h >> 3 & 1;
        int b2 = h >> 2 & 1;
        int b1 = h & 3;

        float p = b1 == 1 ? x : b1 == 2 ? y : z;
        float q = b1 == 1 ? y : b1 == 2 ? z : x;
        float r = b1 == 1 ? z : b1 == 2 ? x : y;

        p = b5 == b3 ? -p : p;
        q = b5 == b4 ? -q : q;
        r = b5 != (b4 ^ b3) ? -r : r;
        t *= t;
        return 8 * t * t * (p + (b1 == 0 ? q + r : b2 == 0 ? q : r));
    }

    int Shuffle(int i, int j, int k) {
        return B(i, j, k, 0) + B(j, k, i, 1) + B(k, i, j, 2) + B(i, j, k, 3) + B(j, k, i, 4) + B(k, i, j, 5) + B(i, j, k, 6) + B(j, k, i, 7);
    }

    int B(int i, int j, int k, int b) {
        return _[B(i, b) << 2 | B(j, b) << 1 | B(k, b)];
    }

    int B(int n, int b) {
        return n >> b & 1;
    }
    
	int Fastfloor(float n) {
	    return n > 0 ? (int)n : (int)n - 1;
	}
}