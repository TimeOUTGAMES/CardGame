using UnityEngine;

public class EndOfEraCard : Cards
{

    public static EndOfEraCard Instance;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Update()
    {
        base.Update();
    }




}
