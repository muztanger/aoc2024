﻿namespace Advent_of_Code_2024.Commons;

public class Box<T> : IEquatable<Box<T>>
    where T : INumber<T>
{
    public Pos<T> Min { get; set; }
    public Pos<T> Max { get; set; }

    public T Width => T.Abs(Max.x - Min.x) + T.One;
    public T Height => T.Abs(Max.y - Min.y) + T.One;
    public T Area => Width * Height;
    public Pos<T> Size => new(Width, Height);

    public Box(params Pos<T>[] positions)
    {
        Assert.IsTrue(positions.Length > 0);
        Min = new Pos<T>(positions[0]);
        Max = new Pos<T>(positions[0]);
        foreach (var p in positions)
        {
            IncreaseToPoint(p);
        }
    }

    public Box(T width, T height) :
        this(new Pos<T>(T.Zero, T.Zero), new Pos<T>(width - T.One, height - T.One))
    {
    }

    public Box(Box<T> other)
    {
        Min = new Pos<T>(other.Min);
        Max = new Pos<T>(other.Max);
    }

    public void IncreaseToPoint(Pos<T> p)
    {
        Min.x = T.Min(Min.x, p.x);
        Min.y = T.Min(Min.y, p.y);
        Max.x = T.Max(Max.x, p.x);
        Max.y = T.Max(Max.y, p.y);
    }

    public override string ToString()
    {
        return $"[{Min}, {Max}]";
    }

    public bool Contains(Pos<T> pos)
    {
        if (pos.x < Min.x || pos.x > Max.x)
        {
            return false;
        }
        if (pos.y < Min.y || pos.y > Max.y)
        {
            return false;
        }
        return true;
    }

    public bool Contains(Box<T> box)
    {
        if (box.Min.x < Min.x || box.Max.x > Max.x)
        {
            return false;
        }
        if (box.Min.y < Min.y || box.Max.y > Max.y)
        {
            return false;
        }
        return true;
    }

    public IEnumerable<Pos<T>> GetPositions()
    {
        for (T y = Min.y; y <= Max.y; y += T.One)
        {
            for (T x = Min.x; x <= Max.x; x += T.One)
            {
                yield return new Pos<T>(x, y);
            }
        }
    }

    public IEnumerable<Pos<T>> GetPositionsByColumn()
    {
        for (T x = Min.x; x <= Max.x; x += T.One)
        {
            for (T y = Min.y; y <= Max.y; y += T.One)
            {
                yield return new Pos<T>(x, y);
            }
        }
    }

    public Box<T>? Intersection(Box<T> other)
    {
        if (Max.x < other.Min.x || other.Max.x < Min.x) return null;
        if (Max.y < other.Min.y || other.Max.y < Min.y) return null;
        var max = new Pos<T>(T.Min(Max.x, other.Max.x), T.Min(Max.y, other.Max.y));
        var min = new Pos<T>(T.Max(Min.x, other.Min.x), T.Max(Min.y, other.Min.y));
        return new Box<T>(min, max);
    }

    public Box<T> Translate(Pos<T> dp)
    {
        var result = new Box<T>(this);
        result.Max += dp;
        result.Min += dp;
        return result;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        Box<T>? posObj = obj as Box<T>;
        if (posObj == null)
            return false;
        else
            return Equals(posObj);
    }

    public bool Equals(Box<T>? other)
    {
        return other != null &&
               Max == other.Max &&
               Min == other.Min;
    }

    public override int GetHashCode()
    {
        return Min.GetHashCode() * 3779 + Max.GetHashCode();
    }

}
