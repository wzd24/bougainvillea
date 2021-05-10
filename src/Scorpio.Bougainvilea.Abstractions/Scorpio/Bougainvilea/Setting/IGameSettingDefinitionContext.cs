namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingDefinitionContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        GameSettingDefinition GetOrNull(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        GameSettingDefinition GetOrNull<T>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingDefinitions"></param>
        void Add(params GameSettingDefinition[] settingDefinitions);
    }
}