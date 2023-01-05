﻿using DormitoryBot.App;

namespace DomitoryBot.App.Interfaces;

public interface IUsersStateRepository
{
    public DialogState GetState(long id);
    public bool ContainsKey(long id);
    public void SetState(long id, DialogState state);
}