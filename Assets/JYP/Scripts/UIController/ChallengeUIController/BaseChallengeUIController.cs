    using System;
    using UnityEngine.UIElements;

    public abstract class BaseChallengeUIController
    {
        public ChallengeUIControllerV2 parentController { get; private set; }
        public VisualElement Root { get; private set; }
        public virtual void Initialize(VisualElement root, ChallengeUIControllerV2 parentController)
        {
            this.parentController = parentController;
            this.Root = root;
        }
        public abstract void BindType(ChallengeType type);
        
    }