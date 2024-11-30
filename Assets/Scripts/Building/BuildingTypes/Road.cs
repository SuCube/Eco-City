// Road.cs

using UnityEngine;

public class Road : Building
{
    //public GridSystem gridSystem; // ¬озможно рудимент

    public enum RoadType
    {
        Straight,
        Turn,
        Intersection,
        TIntersection
    }

    public RoadType CurrentRoadType { get; private set; }

    public Road() : base(12)
    {
        CurrentRoadType = RoadType.Straight; // Ќачальное состо€ние
    }

    // ћетод дл€ обновлени€ состо€ни€ дороги в зависимости от соседних дорог
    public void UpdateRoadType()
    {
        // Ћогика дл€ определени€ типа дороги на основе соседних €чеек
        // Ќапример, провер€ем соседние дороги и мен€ем состо€ние
        // Ёто просто пример, вам нужно будет реализовать свою логику

        // ѕример изменени€ состо€ни€
        CurrentRoadType = DetermineRoadType();
        // ќбновление визуального представлени€ дороги
        UpdateVisuals();
    }

    private RoadType DetermineRoadType()
    {
        // Ћогика дл€ определени€ типа дороги
        // Ќапример, если есть дороги слева и справа, это перекресток
        // ¬ерните соответствующий тип дороги
        return RoadType.Intersection; // Ёто просто пример
    }

    private void UpdateVisuals()
    {
        // ќбновление визуального представлени€ дороги в зависимости от CurrentRoadType
        // Ќапример, можно мен€ть спрайты или модели
        switch (CurrentRoadType)
        {
            case RoadType.Straight:
                // Ћогика дл€ отображени€ пр€мой дороги
                break;
            case RoadType.Turn:
                // Ћогика дл€ отображени€ поворота
                break;
            case RoadType.Intersection:
                // Ћогика дл€ отображени€ перекрестка
                break;
            case RoadType.TIntersection:
                // Ћогика дл€ отображени€ “-образной дороги
                break;
        }
    }
}