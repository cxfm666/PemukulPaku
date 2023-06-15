﻿namespace PemukulPaku.GameServer.Commands
{
    public class ReadLine
    {
        public static ReadLine? Instance { get; private set; }
        public Session? session = null;

        public static ReadLine GetInstance()
        {
            return Instance ??= new();
        }

        public void Start()
        {
            while (true)
            {
                string? line = Console.ReadLine();

                if (!string.IsNullOrEmpty(line))
                {
                    List<string> args = line.Split(' ').ToList();
                    Command? Cmd = CommandFactory.Commands.Find(cmd => args[0] == cmd.Name.ToLower());

                    if(Cmd != null)
                    {
                        args.RemoveAt(0);

                        try
                        {
                            if (Cmd.CmdType == CommandType.All || Cmd.CmdType == CommandType.Console)
                            {
                                Cmd.Run(args.ToArray());
                            }
                            else if(session != null)
                            {
                                Cmd.Run(session, args.ToArray());
                                Command.c.Log("命令执行成功！");
                            }
                            else
                            {
                                Command.c.Error("Invalid usage, try selecting session first with target");
                            }
                        }
                        catch (Exception ex)
                        {
                            Command.c.Error(ex.Message);
                        }

                        continue;
                    }
                    else
                    {
                        Command.c.Error("命令未找到，请使用help查看帮助");
                    }
                }
            }
        }
    }
}
