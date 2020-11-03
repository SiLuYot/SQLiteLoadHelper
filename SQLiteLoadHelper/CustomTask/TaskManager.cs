using System;

namespace SQLiteLoadHelper.CustomTask
{
    //생성된 작업을 처리한다.
    public class TaskManager
    {
        //현재 작업의 진행률
        public double currentProgress;

        //생성된 작업
        private BaseTask task;
        //작업할 데이터 배열
        private object[] dataArray;

        //작업이 진행될 때 마다 호출되는 액션
        private Action<double> taskProgressValue;
        //현재 작업중인 데이터가 변경될때 호출되는 액션
        private Action<string> taskProgressDataName;
        //현재 작업이 끝난 경우 호출되는 액션
        private Action<BaseTask> taskComplete;

        //생성된 작업에 액션들을 추가한다.
        public void AddTaskEvent(Action<double> taskProgressValue, Action<string> taskProgressDataName, Action<BaseTask> taskComplete)
        {
            this.taskProgressValue = taskProgressValue;
            this.taskProgressDataName = taskProgressDataName;
            this.taskComplete = taskComplete;
        }

        //작업 시작
        public bool Start(object[] dataArray, BaseTask task)
        {
            this.dataArray = dataArray;
            this.task = task;

            return Exexute();
        }

        private bool Exexute()
        {
            currentProgress = 0;
            taskProgressValue?.Invoke(currentProgress);

            //dataArray의 인덱스당 한 Step
            for (int i = 0; i < dataArray.Length; i++)
            {
                var name = task.GetName(dataArray[i]);
                taskProgressDataName?.Invoke(name);

                if (!task.Execute(dataArray[i]))
                {
                    return false;
                }

                currentProgress = (int)(((i + 1f) / (float)dataArray?.Length) * 100.0f);
                taskProgressValue?.Invoke(currentProgress);
            }

            taskComplete?.Invoke(task);
            return true;
        }
    }
}
