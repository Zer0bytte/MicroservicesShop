﻿namespace InstagramDMs.API.Vms
{
    public class InstagramWebhookPayload
    {
        public string Object { get; set; }
        public List<Entry> Entry { get; set; }
    }
}
