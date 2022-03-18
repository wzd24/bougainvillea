using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CSRedis
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICSRedisClient
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long Append(string key, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> AppendAsync(string key, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool BfAdd(string key, object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool BfExists(string key, object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        (long capacity, long size, long numberOfFilters, long numberOfItemsInserted, long expansionRate) BfInfo(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <param name="capacity"></param>
        /// <param name="error"></param>
        /// <param name="expansion"></param>
        /// <param name="noCreate"></param>
        /// <param name="nonScaling"></param>
        /// <returns></returns>
        bool[] BfInsert(string key, object[] items, long? capacity = null, string error = null, int expansion = 2, bool noCreate = false, bool nonScaling = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iter"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool BfLoadChunk(string key, long iter, byte[] data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        bool[] BfMAdd(string key, object[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        bool[] BfMExists(string key, object[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errorRate"></param>
        /// <param name="capacity"></param>
        /// <param name="expansion"></param>
        /// <param name="nonScaling"></param>
        /// <returns></returns>
        bool BfReserve(string key, double errorRate, long capacity, int expansion = 2, bool nonScaling = false);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="iter"></param>
        /// <returns></returns>
        RedisScan<byte[]> BfScanDump<T>(string key, long iter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        long BitCount(string key, long start, long end);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<long> BitCountAsync(string key, long start, long end);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="destKey"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        long BitOp(RedisBitOp op, string destKey, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="destKey"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<long> BitOpAsync(RedisBitOp op, string destKey, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bit"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        long BitPos(string key, bool bit, long? start = null, long? end = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bit"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<long> BitPosAsync(string key, bool bit, long? start = null, long? end = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        string BLPop(int timeout, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeout"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        T BLPop<T>(int timeout, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        (string key, string value)? BLPopWithKey(int timeout, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeout"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        (string key, T value)? BLPopWithKey<T>(int timeout, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        string BRPop(int timeout, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeout"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        T BRPop<T>(int timeout, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        string BRPopLPush(string source, string destination, int timeout);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        T BRPopLPush<T>(string source, string destination, int timeout);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        (string key, string value)? BRPopWithKey(int timeout, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeout"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        (string key, T value)? BRPopWithKey<T>(int timeout, params string[] keys);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="getData"></param>
        /// <returns></returns>
        T CacheShell<T>(string key, int timeoutSeconds, Func<T> getData);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="getData"></param>
        /// <returns></returns>
        T CacheShell<T>(string key, string field, int timeoutSeconds, Func<T> getData);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="getData"></param>
        /// <returns></returns>
        (string key, T value)[] CacheShell<T>(string key, string[] fields, int timeoutSeconds, Func<string[], (string, T)[]> getData);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="getDataAsync"></param>
        /// <returns></returns>
        Task<T> CacheShellAsync<T>(string key, int timeoutSeconds, Func<Task<T>> getDataAsync);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="getDataAsync"></param>
        /// <returns></returns>
        Task<T> CacheShellAsync<T>(string key, string field, int timeoutSeconds, Func<Task<T>> getDataAsync);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="getDataAsync"></param>
        /// <returns></returns>
        Task<(string key, T value)[]> CacheShellAsync<T>(string key, string[] fields, int timeoutSeconds, Func<string[], Task<(string, T)[]>> getDataAsync);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CfAdd(string key, object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CfAddNx(string key, object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        long CfCount(string key, object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CfDel(string key, object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CfExists(string key, object item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        (long size, long numberOfBuckets, long numberOfFilter, long numberOfItemsInserted, long numberOfItemsDeleted, long bucketSize, long expansionRate, long maxIteration) CfInfo(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <param name="capacity"></param>
        /// <param name="noCreate"></param>
        /// <returns></returns>
        bool[] CfInsert(string key, object[] items, long? capacity = null, bool noCreate = false);
        bool[] CfInsertNx(string key, object[] items, long? capacity = null, bool noCreate = false);
        bool CfLoadChunk(string key, long iter, byte[] data);
        bool CfReserve(string key, long capacity, long? bucketSize = null, long? maxIterations = null, int? expansion = null);
        RedisScan<byte[]> CfScanDump<T>(string key, long iter);
        long[] CmsIncrBy(string key, params (object item, long increment)[] items);
        (long width, long depth, long count) CmsInfo(string key);
        bool CmsInitByDim(string key, long width, long depth);
        bool CmsInitByProb(string key, double error, double probability);
        bool CmsMerge(string dest, long numKeys, string[] src, long[] weights);
        long[] CmsQuery(string key, params object[] items);
        long Del(params string[] key);
        Task<long> DelAsync(params string[] key);
        void Dispose();
        byte[] Dump(string key);
        Task<byte[]> DumpAsync(string key);
        string Echo(string message);
        string Echo(string nodeKey, string message);
        Task<string> EchoAsync(string message);
        Task<string> EchoAsync(string nodeKey, string message);
        object Eval(string script, string key, params object[] args);
        Task<object> EvalAsync(string script, string key, params object[] args);
        object EvalSHA(string sha1, string key, params object[] args);
        Task<object> EvalSHAAsync(string sha1, string key, params object[] args);
        bool Exists(string key);
        long Exists(string[] keys);
        Task<bool> ExistsAsync(string key);
        Task<long> ExistsAsync(string[] keys);
        bool Expire(string key, int seconds);
        bool Expire(string key, TimeSpan expire);
        Task<bool> ExpireAsync(string key, int seconds);
        Task<bool> ExpireAsync(string key, TimeSpan expire);
        bool ExpireAt(string key, DateTime expire);
        Task<bool> ExpireAtAsync(string key, DateTime expire);
        bool GeoAdd(string key, double longitude, double latitude, object member);
        long GeoAdd(string key, params (double longitude, double latitude, object member)[] values);
        Task<bool> GeoAddAsync(string key, double longitude, double latitude, object member);
        Task<long> GeoAddAsync(string key, params (double longitude, double latitude, object member)[] values);
        double? GeoDist(string key, object member1, object member2, GeoUnit unit = GeoUnit.m);
        Task<double?> GeoDistAsync(string key, object member1, object member2, GeoUnit unit = GeoUnit.m);
        string[] GeoHash(string key, object[] members);
        Task<string[]> GeoHashAsync(string key, object[] members);
        (double longitude, double latitude)?[] GeoPos(string key, object[] members);
        Task<(double longitude, double latitude)?[]> GeoPosAsync(string key, object[] members);
        string[] GeoRadius(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        T[] GeoRadius<T>(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<string[]> GeoRadiusAsync(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<T[]> GeoRadiusAsync<T>(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        string[] GeoRadiusByMember(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        T[] GeoRadiusByMember<T>(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<string[]> GeoRadiusByMemberAsync(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<T[]> GeoRadiusByMemberAsync<T>(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        (string member, double dist)[] GeoRadiusByMemberWithDist(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        (T member, double dist)[] GeoRadiusByMemberWithDist<T>(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        (string member, double dist, double longitude, double latitude)[] GeoRadiusByMemberWithDistAndCoord(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        (T member, double dist, double longitude, double latitude)[] GeoRadiusByMemberWithDistAndCoord<T>(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<(string member, double dist, double longitude, double latitude)[]> GeoRadiusByMemberWithDistAndCoordAsync(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<(T member, double dist, double longitude, double latitude)[]> GeoRadiusByMemberWithDistAndCoordAsync<T>(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<(string member, double dist)[]> GeoRadiusByMemberWithDistAsync(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<(T member, double dist)[]> GeoRadiusByMemberWithDistAsync<T>(string key, object member, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        (string member, double dist)[] GeoRadiusWithDist(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        (T member, double dist)[] GeoRadiusWithDist<T>(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        (string member, double dist, double longitude, double latitude)[] GeoRadiusWithDistAndCoord(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        (T member, double dist, double longitude, double latitude)[] GeoRadiusWithDistAndCoord<T>(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<(string member, double dist, double longitude, double latitude)[]> GeoRadiusWithDistAndCoordAsync(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<(T member, double dist, double longitude, double latitude)[]> GeoRadiusWithDistAndCoordAsync<T>(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<(string member, double dist)[]> GeoRadiusWithDistAsync(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        Task<(T member, double dist)[]> GeoRadiusWithDistAsync<T>(string key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null);
        string Get(string key);
        void Get(string key, Stream destination, int bufferSize = 1024);
        T Get<T>(string key);
        Task<string> GetAsync(string key);
        Task<T> GetAsync<T>(string key);
        bool GetBit(string key, uint offset);
        Task<bool> GetBitAsync(string key, uint offset);
        string GetRange(string key, long start, long end);
        T GetRange<T>(string key, long start, long end);
        Task<string> GetRangeAsync(string key, long start, long end);
        Task<T> GetRangeAsync<T>(string key, long start, long end);
        string GetSet(string key, object value);
        T GetSet<T>(string key, object value);
        Task<string> GetSetAsync(string key, object value);
        Task<T> GetSetAsync<T>(string key, object value);
        long HDel(string key, params string[] fields);
        Task<long> HDelAsync(string key, params string[] fields);
        bool HExists(string key, string field);
        Task<bool> HExistsAsync(string key, string field);
        string HGet(string key, string field);
        T HGet<T>(string key, string field);
        Dictionary<string, string> HGetAll(string key);
        Dictionary<string, T> HGetAll<T>(string key);
        Task<Dictionary<string, string>> HGetAllAsync(string key);
        Task<Dictionary<string, T>> HGetAllAsync<T>(string key);
        Task<string> HGetAsync(string key, string field);
        Task<T> HGetAsync<T>(string key, string field);
        long HIncrBy(string key, string field, long value = 1);
        Task<long> HIncrByAsync(string key, string field, long value = 1);
        double HIncrByFloat(string key, string field, double value);
        Task<double> HIncrByFloatAsync(string key, string field, double value);
        string[] HKeys(string key);
        Task<string[]> HKeysAsync(string key);
        long HLen(string key);
        Task<long> HLenAsync(string key);
        string[] HMGet(string key, params string[] fields);
        T[] HMGet<T>(string key, params string[] fields);
        Task<string[]> HMGetAsync(string key, params string[] fields);
        Task<T[]> HMGetAsync<T>(string key, params string[] fields);
        bool HMSet(string key, params object[] keyValues);
        Task<bool> HMSetAsync(string key, params object[] keyValues);
        RedisScan<(string field, string value)> HScan(string key, long cursor, string pattern = null, long? count = null);
        RedisScan<(string field, T value)> HScan<T>(string key, long cursor, string pattern = null, long? count = null);
        Task<RedisScan<(string field, string value)>> HScanAsync(string key, long cursor, string pattern = null, long? count = null);
        Task<RedisScan<(string field, T value)>> HScanAsync<T>(string key, long cursor, string pattern = null, long? count = null);
        bool HSet(string key, string field, object value);
        Task<bool> HSetAsync(string key, string field, object value);
        bool HSetNx(string key, string field, object value);
        Task<bool> HSetNxAsync(string key, string field, object value);
        long HStrLen(string key, string field);
        Task<long> HStrLenAsync(string key, string field);
        string[] HVals(string key);
        T[] HVals<T>(string key);
        Task<string[]> HValsAsync(string key);
        Task<T[]> HValsAsync<T>(string key);
        long IncrBy(string key, long value = 1);
        Task<long> IncrByAsync(string key, long value = 1);
        double IncrByFloat(string key, double value);
        Task<double> IncrByFloatAsync(string key, double value);
        string[] Keys(string pattern);
        Task<string[]> KeysAsync(string pattern);
        string LIndex(string key, long index);
        T LIndex<T>(string key, long index);
        Task<string> LIndexAsync(string key, long index);
        Task<T> LIndexAsync<T>(string key, long index);
        long LInsertAfter(string key, object pivot, object value);
        Task<long> LInsertAfterAsync(string key, object pivot, object value);
        long LInsertBefore(string key, object pivot, object value);
        Task<long> LInsertBeforeAsync(string key, object pivot, object value);
        long LLen(string key);
        Task<long> LLenAsync(string key);
        string LPop(string key);
        T LPop<T>(string key);
        Task<string> LPopAsync(string key);
        Task<T> LPopAsync<T>(string key);
        long LPush<T>(string key, params T[] value);
        Task<long> LPushAsync<T>(string key, params T[] value);
        long LPushX(string key, object value);
        Task<long> LPushXAsync(string key, object value);
        string[] LRange(string key, long start, long stop);
        T[] LRange<T>(string key, long start, long stop);
        Task<string[]> LRangeAsync(string key, long start, long stop);
        Task<T[]> LRangeAsync<T>(string key, long start, long stop);
        long LRem(string key, long count, object value);
        Task<long> LRemAsync(string key, long count, object value);
        bool LSet(string key, long index, object value);
        Task<bool> LSetAsync(string key, long index, object value);
        bool LTrim(string key, long start, long stop);
        Task<bool> LTrimAsync(string key, long start, long stop);
        string[] MGet(params string[] keys);
        T[] MGet<T>(params string[] keys);
        Task<string[]> MGetAsync(params string[] keys);
        Task<T[]> MGetAsync<T>(params string[] keys);
        bool Move(string key, int database);
        Task<bool> MoveAsync(string key, int database);
        bool MSet(params object[] keyValues);
        Task<bool> MSetAsync(params object[] keyValues);
        bool MSetNx(params object[] keyValues);
        Task<bool> MSetNxAsync(params object[] keyValues);
        string ObjectEncoding(string key);
        Task<string> ObjectEncodingAsync(string key);
        long? ObjectIdleTime(string key);
        Task<long?> ObjectIdleTimeAsync(string key);
        long? ObjectRefCount(string key);
        Task<long?> ObjectRefCountAsync(string key);
        bool Persist(string key);
        Task<bool> PersistAsync(string key);
        bool PExpire(string key, int milliseconds);
        bool PExpire(string key, TimeSpan expire);
        Task<bool> PExpireAsync(string key, int milliseconds);
        Task<bool> PExpireAsync(string key, TimeSpan expire);
        bool PExpireAt(string key, DateTime expire);
        Task<bool> PExpireAtAsync(string key, DateTime expire);
        bool PfAdd<T>(string key, params T[] elements);
        Task<bool> PfAddAsync<T>(string key, params T[] elements);
        long PfCount(params string[] keys);
        Task<long> PfCountAsync(params string[] keys);
        bool PfMerge(string destKey, params string[] sourceKeys);
        Task<bool> PfMergeAsync(string destKey, params string[] sourceKeys);
        bool Ping();
        bool Ping(string nodeKey);
        Task<bool> PingAsync();
        Task<bool> PingAsync(string nodeKey);
        long PTtl(string key);
        Task<long> PTtlAsync(string key);
        long Publish(string channel, string message);
        Task<long> PublishAsync(string channel, string message);
        long PublishNoneMessageId(string channel, string message);
        Task<long> PublishNoneMessageIdAsync(string channel, string message);
        string[] PubSubChannels(string pattern);
        Task<string[]> PubSubChannelsAsync(string pattern);
        long PubSubNumPat();
        Task<long> PubSubNumPatAsync();
        Dictionary<string, long> PubSubNumSub(params string[] channels);
        Task<Dictionary<string, long>> PubSubNumSubAsync(params string[] channels);
        string RandomKey();
        Task<string> RandomKeyAsync();
        bool Rename(string key, string newKey);
        Task<bool> RenameAsync(string key, string newKey);
        bool RenameNx(string key, string newKey);
        Task<bool> RenameNxAsync(string key, string newKey);
        bool Restore(string key, byte[] serializedValue);
        bool Restore(string key, long ttlMilliseconds, byte[] serializedValue);
        Task<bool> RestoreAsync(string key, byte[] serializedValue);
        Task<bool> RestoreAsync(string key, long ttlMilliseconds, byte[] serializedValue);
        string RPop(string key);
        T RPop<T>(string key);
        Task<string> RPopAsync(string key);
        Task<T> RPopAsync<T>(string key);
        string RPopLPush(string source, string destination);
        T RPopLPush<T>(string source, string destination);
        Task<string> RPopLPushAsync(string source, string destination);
        Task<T> RPopLPushAsync<T>(string source, string destination);
        long RPush<T>(string key, params T[] value);
        Task<long> RPushAsync<T>(string key, params T[] value);
        long RPushX(string key, object value);
        Task<long> RPushXAsync(string key, object value);
        long SAdd<T>(string key, params T[] members);
        Task<long> SAddAsync<T>(string key, params T[] members);
        RedisScan<string> Scan(long cursor, string pattern = null, long? count = null);
        RedisScan<T> Scan<T>(long cursor, string pattern = null, long? count = null);
        Task<RedisScan<string>> ScanAsync(long cursor, string pattern = null, long? count = null);
        Task<RedisScan<T>> ScanAsync<T>(long cursor, string pattern = null, long? count = null);
        long SCard(string key);
        Task<long> SCardAsync(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sha1"></param>
        /// <returns></returns>
        bool[] ScriptExists(params string[] sha1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sha1"></param>
        /// <returns></returns>
        Task<bool[]> ScriptExistsAsync(params string[] sha1);
        
        /// <summary>
        /// 
        /// </summary>
        void ScriptFlush();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ScriptFlushAsync();
        
        /// <summary>
        /// 
        /// </summary>
        void ScriptKill();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ScriptKillAsync();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        string ScriptLoad(string script);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        Task<string> ScriptLoadAsync(string script);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        string[] SDiff(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] SDiff<T>(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<string[]> SDiffAsync(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> SDiffAsync<T>(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        long SDiffStore(string destination, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<long> SDiffStoreAsync(string destination, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="exists"></param>
        /// <returns></returns>
        bool Set(string key, object value, int expireSeconds = -1, RedisExistence? exists = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <param name="exists"></param>
        /// <returns></returns>
        bool Set(string key, object value, TimeSpan expire, RedisExistence? exists = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="exists"></param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, object value, int expireSeconds = -1, RedisExistence? exists = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <param name="exists"></param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, object value, TimeSpan expire, RedisExistence? exists = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetBit(string key, uint offset, bool value);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetBitAsync(string key, uint offset, bool value);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetNx(string key, object value);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetNxAsync(string key, object value);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long SetRange(string key, uint offset, object value);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> SetRangeAsync(string key, uint offset, object value);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        string[] SInter(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] SInter<T>(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<string[]> SInterAsync(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> SInterAsync<T>(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        long SInterStore(string destination, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<long> SInterStoreAsync(string destination, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        bool SIsMember(string key, object member);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<bool> SIsMemberAsync(string key, object member);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string[] SMembers(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T[] SMembers<T>(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string[]> SMembersAsync(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T[]> SMembersAsync<T>(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        bool SMove(string source, string destination, object member);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<bool> SMoveAsync(string source, string destination, object member);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="by"></param>
        /// <param name="dir"></param>
        /// <param name="isAlpha"></param>
        /// <param name="get"></param>
        /// <returns></returns>
        string[] Sort(string key, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="destination"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="by"></param>
        /// <param name="dir"></param>
        /// <param name="isAlpha"></param>
        /// <param name="get"></param>
        /// <returns></returns>
        long SortAndStore(string key, string destination, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="destination"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="by"></param>
        /// <param name="dir"></param>
        /// <param name="isAlpha"></param>
        /// <param name="get"></param>
        /// <returns></returns>
        Task<long> SortAndStoreAsync(string key, string destination, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="by"></param>
        /// <param name="dir"></param>
        /// <param name="isAlpha"></param>
        /// <param name="get"></param>
        /// <returns></returns>
        Task<string[]> SortAsync(string key, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string SPop(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string[] SPop(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T SPop<T>(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        T[] SPop<T>(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> SPopAsync(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<string[]> SPopAsync(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> SPopAsync<T>(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<T[]> SPopAsync<T>(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string SRandMember(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T SRandMember<T>(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> SRandMemberAsync(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> SRandMemberAsync<T>(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string[] SRandMembers(string key, int count = 1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        T[] SRandMembers<T>(string key, int count = 1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<string[]> SRandMembersAsync(string key, int count = 1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<T[]> SRandMembersAsync<T>(string key, int count = 1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        long SRem<T>(string key, params T[] members);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        Task<long> SRemAsync<T>(string key, params T[] members);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        RedisScan<string> SScan(string key, long cursor, string pattern = null, long? count = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        RedisScan<T> SScan<T>(string key, long cursor, string pattern = null, long? count = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<RedisScan<string>> SScanAsync(string key, long cursor, string pattern = null, long? count = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<RedisScan<T>> SScanAsync<T>(string key, long cursor, string pattern = null, long? count = null);

        ICSRedisClientPipe<string> StartPipe();

        object[] StartPipe(Action<ICSRedisClientPipe<string>> handler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long StrLen(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> StrLenAsync(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        string[] SUnion(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        T[] SUnion<T>(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<string[]> SUnionAsync(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T[]> SUnionAsync<T>(params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        long SUnionStore(string destination, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<long> SUnionStoreAsync(string destination, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        string[] TopkAdd(string key, object[] items);
       /// <summary>
       /// 
       /// </summary>
       /// <param name="key"></param>
       /// <param name="items"></param>
       /// <returns></returns>
        long[] TopkCount(string key, object[] items);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        string[] TopkIncrBy(string key, params (object item, long increment)[] items);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        (long k, long width, long depth, double decay) TopkInfo(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string[] TopkList(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        bool[] TopkQuery(string key, object[] items);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="topk"></param>
        /// <param name="width"></param>
        /// <param name="depth"></param>
        /// <param name="decay"></param>
        /// <returns></returns>
        bool TopkReserve(string key, long topk, long width, long depth, double decay);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long Touch(params string[] key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> TouchAsync(params string[] key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long Ttl(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> TtlAsync(string key);
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long UnLink(params string[] key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> UnLinkAsync(params string[] key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        long XAck(string key, string group, string id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <param name="fieldValues"></param>
        /// <returns></returns>
        string XAdd(string key, string id = "*", params (string, string)[] fieldValues);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="maxLen"></param>
        /// <param name="id"></param>
        /// <param name="fieldValues"></param>
        /// <returns></returns>
        string XAdd(string key, long maxLen, string id = "*", params (string, string)[] fieldValues);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldValues"></param>
        /// <returns></returns>
        string XAdd(string key, params (string, string)[] fieldValues);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="consumer"></param>
        /// <param name="minIdleTime"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        (string id, string[] items)[] XClaim(string key, string group, string consumer, long minIdleTime, params string[] id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="consumer"></param>
        /// <param name="minIdleTime"></param>
        /// <param name="id"></param>
        /// <param name="idle"></param>
        /// <param name="retryCount"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        (string id, string[] items)[] XClaim(string key, string group, string consumer, long minIdleTime, string[] id, long idle, long retryCount, bool force);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="consumer"></param>
        /// <param name="minIdleTime"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string[] XClaimJustId(string key, string group, string consumer, long minIdleTime, params string[] id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="consumer"></param>
        /// <param name="minIdleTime"></param>
        /// <param name="id"></param>
        /// <param name="idle"></param>
        /// <param name="retryCount"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        string[] XClaimJustId(string key, string group, string consumer, long minIdleTime, string[] id, long idle, long retryCount, bool force);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        long XDel(string key, params string[] id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <param name="MkStream"></param>
        /// <returns></returns>
        string XGroupCreate(string key, string group, string id = "$", bool MkStream = false);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="consumer"></param>
        /// <returns></returns>
        bool XGroupDelConsumer(string key, string group, string consumer);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        bool XGroupDestroy(string key, string group);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string XGroupSetId(string key, string group, string id = "$");
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        (string name, long pending, long idle)[] XInfoConsumers(string key, string group);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        (string name, long consumers, long pending, string lastDeliveredId)[] XInfoGroups(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        (long length, long radixTreeKeys, long radixTreeNodes, long groups, string lastGeneratedId, (string id, string[] items) firstEntry, (string id, string[] items) lastEntry) XInfoStream(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long XLen(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        (long count, string minId, string maxId, (string consumer, long count)[] pendings) XPending(string key, string group);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="count"></param>
        /// <param name="consumer"></param>
        /// <returns></returns>
        (string id, string consumer, long idle, long transferTimes)[] XPending(string key, string group, string start, string end, long count, string consumer = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        (string id, string[] items)[] XRange(string key, string start, string end, long count = 1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="block"></param>
        /// <param name="streams"></param>
        /// <returns></returns>
        (string key, (string id, string[] items)[] data)[] XRead(long count, long block, params (string key, string id)[] streams);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="consumer"></param>
        /// <param name="count"></param>
        /// <param name="block"></param>
        /// <param name="streams"></param>
        /// <returns></returns>
        (string key, (string id, string[] items)[] data)[] XReadGroup(string group, string consumer, long count, long block, params (string key, string id)[] streams);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="end"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        (string id, string[] items)[] XRevRange(string key, string end, string start, long count = 1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="maxLen"></param>
        /// <returns></returns>
        long XTrim(string key, long maxLen);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scoreMembers"></param>
        /// <returns></returns>
        long ZAdd(string key, params (double, object)[] scoreMembers);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scoreMembers"></param>
        /// <returns></returns>
        Task<long> ZAddAsync(string key, params (double, object)[] scoreMembers);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ZCard(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<long> ZCardAsync(string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZCount(string key, double min, double max);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZCount(string key, string min, string max);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Task<long> ZCountAsync(string key, double min, double max);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Task<long> ZCountAsync(string key, string min, string max);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        double ZIncrBy(string key, object member, double increment = 1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        Task<double> ZIncrByAsync(string key, string member, double increment = 1);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="weights"></param>
        /// <param name="aggregate"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        long ZInterStore(string destination, double[] weights, RedisAggregate aggregate, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="weights"></param>
        /// <param name="aggregate"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<long> ZInterStoreAsync(string destination, double[] weights, RedisAggregate aggregate, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZLexCount(string key, string min, string max);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Task<long> ZLexCountAsync(string key, string min, string max);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        (string member, double score)[] ZPopMax(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        (T member, double score)[] ZPopMax<T>(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<(string member, double score)[]> ZPopMaxAsync(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<(T member, double score)[]> ZPopMaxAsync<T>(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        (string member, double score)[] ZPopMin(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        (T member, double score)[] ZPopMin<T>(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<(string member, double score)[]> ZPopMinAsync(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<(T member, double score)[]> ZPopMinAsync<T>(string key, long count);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        string[] ZRange(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        T[] ZRange<T>(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<string[]> ZRangeAsync(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<T[]> ZRangeAsync<T>(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        string[] ZRangeByLex(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        T[] ZRangeByLex<T>(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<string[]> ZRangeByLexAsync(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<T[]> ZRangeByLexAsync<T>(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        string[] ZRangeByScore(string key, double min, double max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        string[] ZRangeByScore(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        T[] ZRangeByScore<T>(string key, double min, double max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        T[] ZRangeByScore<T>(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<string[]> ZRangeByScoreAsync(string key, double min, double max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<string[]> ZRangeByScoreAsync(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<T[]> ZRangeByScoreAsync<T>(string key, double min, double max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<T[]> ZRangeByScoreAsync<T>(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        (string member, double score)[] ZRangeByScoreWithScores(string key, double min, double max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        (string member, double score)[] ZRangeByScoreWithScores(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        (T member, double score)[] ZRangeByScoreWithScores<T>(string key, double min, double max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        (T member, double score)[] ZRangeByScoreWithScores<T>(string key, string min, string max, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<(string member, double score)[]> ZRangeByScoreWithScoresAsync(string key, double min, double max, long? count = null, long offset = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<(string member, double score)[]> ZRangeByScoreWithScoresAsync(string key, string min, string max, long? count = null, long offset = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<(T member, double score)[]> ZRangeByScoreWithScoresAsync<T>(string key, double min, double max, long? count = null, long offset = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<(T member, double score)[]> ZRangeByScoreWithScoresAsync<T>(string key, string min, string max, long? count = null, long offset = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        (string member, double score)[] ZRangeWithScores(string key, long start, long stop);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        (T member, double score)[] ZRangeWithScores<T>(string key, long start, long stop);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<(string member, double score)[]> ZRangeWithScoresAsync(string key, long start, long stop);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<(T member, double score)[]> ZRangeWithScoresAsync<T>(string key, long start, long stop);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        long? ZRank(string key, object member);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<long?> ZRankAsync(string key, object member);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        long ZRem<T>(string key, params T[] member);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<long> ZRemAsync<T>(string key, params T[] member);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZRemRangeByLex(string key, string min, string max);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Task<long> ZRemRangeByLexAsync(string key, string min, string max);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        long ZRemRangeByRank(string key, long start, long stop);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<long> ZRemRangeByRankAsync(string key, long start, long stop);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZRemRangeByScore(string key, double min, double max);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        long ZRemRangeByScore(string key, string min, string max);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Task<long> ZRemRangeByScoreAsync(string key, double min, double max);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Task<long> ZRemRangeByScoreAsync(string key, string min, string max);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        string[] ZRevRange(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        T[] ZRevRange<T>(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<string[]> ZRevRangeAsync(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<T[]> ZRevRangeAsync<T>(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        string[] ZRevRangeByScore(string key, double max, double min, long? count = null, long? offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        string[] ZRevRangeByScore(string key, string max, string min, long? count = null, long? offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        T[] ZRevRangeByScore<T>(string key, double max, double min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        T[] ZRevRangeByScore<T>(string key, string max, string min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<string[]> ZRevRangeByScoreAsync(string key, double max, double min, long? count = null, long? offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<string[]> ZRevRangeByScoreAsync(string key, string max, string min, long? count = null, long? offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<T[]> ZRevRangeByScoreAsync<T>(string key, double max, double min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<T[]> ZRevRangeByScoreAsync<T>(string key, string max, string min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        (string member, double score)[] ZRevRangeByScoreWithScores(string key, double max, double min, long? count = null, long offset = 0);
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        (string member, double score)[] ZRevRangeByScoreWithScores(string key, string max, string min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        (T member, double score)[] ZRevRangeByScoreWithScores<T>(string key, double max, double min, long? count = null, long offset = 0);
       
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        (T member, double score)[] ZRevRangeByScoreWithScores<T>(string key, string max, string min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<(string member, double score)[]> ZRevRangeByScoreWithScoresAsync(string key, double max, double min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<(string member, double score)[]> ZRevRangeByScoreWithScoresAsync(string key, string max, string min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<(T member, double score)[]> ZRevRangeByScoreWithScoresAsync<T>(string key, double max, double min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        Task<(T member, double score)[]> ZRevRangeByScoreWithScoresAsync<T>(string key, string max, string min, long? count = null, long offset = 0);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        (string member, double score)[] ZRevRangeWithScores(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        (T member, double score)[] ZRevRangeWithScores<T>(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<(string member, double score)[]> ZRevRangeWithScoresAsync(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<(T member, double score)[]> ZRevRangeWithScoresAsync<T>(string key, long start, long stop);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        long? ZRevRank(string key, object member);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<long?> ZRevRankAsync(string key, object member);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        RedisScan<(string member, double score)> ZScan(string key, long cursor, string pattern = null, long? count = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        RedisScan<(T member, double score)> ZScan<T>(string key, long cursor, string pattern = null, long? count = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<RedisScan<(string member, double score)>> ZScanAsync(string key, long cursor, string pattern = null, long? count = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cursor"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<RedisScan<(T member, double score)>> ZScanAsync<T>(string key, long cursor, string pattern = null, long? count = null);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        double? ZScore(string key, object member);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<double?> ZScoreAsync(string key, object member);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="weights"></param>
        /// <param name="aggregate"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        long ZUnionStore(string destination, double[] weights, RedisAggregate aggregate, params string[] keys);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="weights"></param>
        /// <param name="aggregate"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<long> ZUnionStoreAsync(string destination, double[] weights, RedisAggregate aggregate, params string[] keys);
    }
}