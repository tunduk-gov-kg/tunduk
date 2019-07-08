drop function if exists "GetMemberProducedServices"(bigint, bigint, varchar, varchar, varchar);

drop function if exists "GetMemberConsumedServices"(bigint, bigint, varchar, varchar, varchar);

drop function if exists "GetMessagesCountGroupedByDateTime"(timestamp, timestamp, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, boolean, boolean);

create or replace function get_messages_grouped_by_date_time("from" timestamp without time zone,
                                                             "to" timestamp without time zone,
                                                             group_by character varying,
                                                             consumer_instance character varying,
                                                             consumer_member_class character varying,
                                                             consumer_member_code character varying,
                                                             consumer_sub_system_code character varying,
                                                             producer_instance character varying,
                                                             producer_member_class character varying,
                                                             producer_member_code character varying,
                                                             producer_sub_system_code character varying,
                                                             producer_service_code character varying,
                                                             producer_service_version character varying,
                                                             succeeded boolean,
                                                             include_meta_services boolean)
    returns TABLE
            (
                "DateTime"      timestamp without time zone,
                "MessagesCount" bigint
            )
    language plpgsql
as
$$
begin
    return query select date_trunc(group_by,
                                   COALESCE("ConsumerServerRequestIn", "ProducerServerRequestIn")) as "date_time",
                        count(*)                                                                   as "messages_count"
                 from "Messages"

                 where COALESCE("ConsumerServerRequestIn", "ProducerServerRequestIn") >= "from"
                   and COALESCE("ConsumerServerRequestIn", "ProducerServerRequestIn") < "to"

                   and (consumer_instance isnull or consumer_instance = "ConsumerInstance")
                   and (consumer_member_class isnull or consumer_member_class = "ConsumerMemberClass")
                   and (consumer_member_code isnull or consumer_member_code = "ConsumerMemberCode")
                   and (consumer_sub_system_code isnull or consumer_sub_system_code = "ConsumerSubSystemCode")

                   and (producer_instance isnull or producer_instance = "ProducerInstance")
                   and (producer_member_class isnull or producer_member_class = "ProducerMemberClass")
                   and (producer_member_code isnull or producer_member_code = "ProducerMemberCode")
                   and (producer_sub_system_code isnull or producer_sub_system_code = "ProducerSubSystemCode")
                   and (producer_service_code isnull or producer_service_code = "ProducerServiceCode")
                   and (producer_service_version isnull or producer_service_version = "ProducerServiceVersion")
                   and ("succeeded" isnull or "succeeded" = "IsSucceeded")
                   and (include_meta_services is null or CASE
                                                             when not include_meta_services
                                                                 then ("ProducerServiceCode" not in (select * from "MetaServices"))
                                                             else true end)

                 group by date_trunc(group_by,
                                     COALESCE("ConsumerServerRequestIn", "ProducerServerRequestIn"));

end;
$$;

alter function "get_messages_grouped_by_date_time"(timestamp, timestamp, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, varchar, boolean, boolean) owner to postgres;

