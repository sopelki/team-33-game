using Core;
using Logic.Castle;
using Logic.Tower;
using Logic.Trap;
using Logic.Unit;
using UI;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace Misc
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Ссылки на элементы")]
        [SerializeField] private GameObject tutorialWindow;   
        [SerializeField] private DialogueAnimator dialogueAnimator;
        [SerializeField] private Button actionButton;         
        [SerializeField] private TMPro.TextMeshProUGUI actionButtonText; 
        [SerializeField] private GameObject barrackSlot, towerSlot, trapSlot;    
        [SerializeField] private GameObject highlightEffect; 

        private enum TutorialStep { Greeting, BuildBarrack, BuildTower, BuildTrap, Finish }
        private TutorialStep currentStep = TutorialStep.Greeting;

        private GameFlowManager gameFlowManager; 
        private bool barrackTracked, towerTracked, trapTracked;

        // Этот метод нужно вызвать из GameInitializer при старте игры
        public void Setup(GameFlowManager flowManager) => this.gameFlowManager = flowManager;

        private void Start()
        {
            if (actionButton != null) actionButton.onClick.AddListener(OnActionButtonClick);
            UpdateTutorialState();
        }
        public static bool IsTutorialActive()
        {
            var tutorial = FindAnyObjectByType<TutorialManager>();
            return tutorial != null && tutorial.isActiveAndEnabled;
        }

        public void OnActionButtonClick()
        {
            if (currentStep == TutorialStep.Greeting) { currentStep = TutorialStep.BuildBarrack; UpdateTutorialState(); }
            else if (currentStep == TutorialStep.Finish) { FinishTutorialAndStartRealGame(); }
        }

        private void Update()
        {
            // Проверка построек через публичные методы, которые мы добавили в Шаге 1
            if (currentStep == TutorialStep.BuildBarrack && !barrackTracked)
            {
                if (CastleSystem.Instance != null && CastleSystem.Instance.Model.Buildings.Count > 0)
                {
                    barrackTracked = true;
                    NextStepAfterBuild("Отлично, барак готов! Теперь выберите Башню.", TutorialStep.BuildTower);
                }
            }

            if (currentStep == TutorialStep.BuildTower && !towerTracked)
            {
                if (TowerSystem.Instance != null && TowerSystem.Instance.GetTowers().Count > 0) 
                {
                    towerTracked = true;
                    NextStepAfterBuild("Защита установлена! Последний штрих — Ловушка.", TutorialStep.BuildTrap);
                }
            }

            if (currentStep == TutorialStep.BuildTrap && !trapTracked)
            {
                if (TrapSystem.Instance != null && TrapSystem.Instance.GetTraps().Count > 0) 
                {
                    trapTracked = true;
                    currentStep = TutorialStep.Finish;
                    UpdateTutorialState();
                }
            }
        }

        private void NextStepAfterBuild(string text, TutorialStep nextStep)
        {
            if (tutorialWindow != null) tutorialWindow.SetActive(true);
            if (actionButton != null) actionButton.gameObject.SetActive(false); 
            if (dialogueAnimator != null) dialogueAnimator.PrintPhrase(text);
        
            Invoke(nameof(ExecuteNextStep), 3f);
            currentStep = nextStep;
        }

        private void ExecuteNextStep() => UpdateTutorialState();

        private void UpdateTutorialState()
        {
            if (highlightEffect != null) highlightEffect.SetActive(false);
            if (tutorialWindow != null && (currentStep == TutorialStep.Greeting || currentStep == TutorialStep.Finish)) 
                tutorialWindow.SetActive(true);

            switch (currentStep)
            {
                case TutorialStep.Greeting:
                    if (actionButton != null) actionButton.gameObject.SetActive(true);
                    if (actionButtonText != null) actionButtonText.text = "Далее";
                    if (dialogueAnimator != null) dialogueAnimator.PrintPhrase("Лорд, давайте начнём строительство! Перетяните казарму в поле замка");
                    break;
                case TutorialStep.BuildBarrack:
                    // if (tutorialWindow != null) tutorialWindow.SetActive(false);
                    if (highlightEffect != null && barrackSlot != null)
                    {
                        highlightEffect.transform.position = barrackSlot.transform.position; 
                        highlightEffect.SetActive(true);
                    }
                    break;
                case TutorialStep.BuildTower:
                    // if (tutorialWindow != null) tutorialWindow.SetActive(false);
                    if (highlightEffect != null && towerSlot != null)
                    {
                        highlightEffect.transform.position = towerSlot.transform.position; 
                        highlightEffect.SetActive(true);
                    }
                    break;
                case TutorialStep.BuildTrap:
                    // if (tutorialWindow != null) tutorialWindow.SetActive(false);
                    if (highlightEffect != null && trapSlot != null) 
                    { 
                        highlightEffect.transform.position = trapSlot.transform.position; 
                        highlightEffect.SetActive(true); 
                    }
                    break;
                case TutorialStep.Finish:
                    if (actionButton != null) actionButton.gameObject.SetActive(true);
                    if (actionButtonText != null) actionButtonText.text = "К игре!"; 
                    if (dialogueAnimator != null) dialogueAnimator.PrintPhrase("Сборка завершена. Нажмите кнопку, чтобы начать настоящий бой!");
                    break;
            }
        }

        private void FinishTutorialAndStartRealGame()
        {
            if (gameFlowManager != null) 
                gameFlowManager.IsTutorialActive = false;
            
            if (CastleSystem.Instance != null)
            {
                CastleSystem.Instance.Clear();
                foreach (var b in FindObjectsByType<InventoryItem>(FindObjectsInactive.Exclude))
                    Destroy(b.gameObject);

            }
            if (TowerSystem.Instance != null)
            {
                FindAnyObjectByType<TowerViewManager>()?.DestroyAllTowers();
                TowerSystem.Instance.Clear();
            }
            if (TrapSystem.Instance != null)
            {
                FindAnyObjectByType<TrapViewManager>()?.DestroyAllTraps();
                TrapSystem.Instance.Clear();
            }
            if (UnitSystem.Instance != null)
            {
                FindAnyObjectByType<UnitViewManager>()?.DestroyAllUnits();
                UnitSystem.Instance.Clear();
            }

            
            if (gameFlowManager != null) 
            {
                gameFlowManager.ResetToStandardMode(); 
            }

            if (tutorialWindow != null) 
                tutorialWindow.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
    }
}